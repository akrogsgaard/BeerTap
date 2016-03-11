using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using BeerTap.DataPersistance.Entities;
using BeerTap.DomainServices.Tap;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common.Mapping;
using IQ.Platform.Messaging.Models;

namespace BeerTap.DataPersistance.Repositories.Tap
{
    public class TapRepository : ITapRepository
    {
        private readonly IDbContextFactory<BeerTapContext> _contextFactory;
        private readonly IMapper<TapRecord, TapDto> _dtoMapper;
        private readonly IMapper<TapDto, TapRecord> _recordMapper;

        private readonly Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public TapRepository(IDbContextFactory<BeerTapContext> contextFactory,
            IMapper<TapRecord, TapDto> dtoMapper,
            IMapper<TapDto, TapRecord> recordMapper)
        {
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));
            if (dtoMapper == null) throw new ArgumentNullException(nameof(dtoMapper));
            if (recordMapper == null) throw new ArgumentNullException(nameof(recordMapper));
            _contextFactory = contextFactory;
            _dtoMapper = dtoMapper;
            _recordMapper = recordMapper;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<TapDto> GetByIdAsync(int id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                var tapRecord = await context.Taps.FindAsync(id).ConfigureAwait(false);
                return tapRecord == null
                           ? null
                           : _dtoMapper.Map(tapRecord);
            }
        }

        public async Task<IEnumerable<TapDto>> GetAllTapsByOfficeIdAsync(int officeId)
        {
            var tapDtoRecords = new List<TapDto>();

            using (var context = _contextFactory.CreateContext())
            {
                var query = from p in context.Taps
                            where p.OfficeId == officeId
                            select p;

                await query.ForEachAsync(x => tapDtoRecords.Add(_dtoMapper.Map(x)));
                return tapDtoRecords;
            }
        }

        public async Task<int> SaveNewAsync(TapDto TapDto)
        {
            try
            {
                var tapRecord = _recordMapper.Map(TapDto);

                using (var context = _contextFactory.CreateContext())
                {
                    context.Taps.Add(tapRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    return tapRecord.Id;
                }
            }
            catch (DbUpdateException ex)
            {
                var exception = DbContextUtils.ConvertDbUpdateException(ex);
                Logger.Error(new ExpandableLogMessage(exception.ToString(), new KeyValuePair<string, object>("failureMessage", exception.ToString())));
                throw exception;
            }
        }

        public async Task UpdateAsync(TapDto tapDto)
        {
            try
            {
                using (var context = _contextFactory.CreateContext())
                {
                    var tapRecord = await context.Taps.FindAsync(tapDto.Id).ConfigureAwait(false);

                    tapRecord.KegId = tapDto.KegId;
                    tapRecord.UpdatedByUserId = tapDto.UpdatedByUserId;
                    tapRecord.UpdatedDateUtc = tapDto.UpdatedDateUtc;

                    context.Taps.Add(tapRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (DbUpdateException ex)
            {
                var exception = DbContextUtils.ConvertDbUpdateException(ex);
                Logger.Error(new ExpandableLogMessage(exception.ToString(), new KeyValuePair<string, object>("failureMessage", exception.ToString())));
                throw exception;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                using (var context = _contextFactory.CreateContext())
                {
                    var taxPartnerConfiguration = await context.Taps.FindAsync(id).ConfigureAwait(false);

                    if (taxPartnerConfiguration == null)
                        return;

                    context.Taps.Remove(taxPartnerConfiguration);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (DbUpdateException ex)
            {
                var exception = DbContextUtils.ConvertDbUpdateException(ex);
                Logger.Error(new ExpandableLogMessage(exception.ToString(), new KeyValuePair<string, object>("failureMessage", exception.ToString())));
                throw exception;
            }
        }

        
    }
}
