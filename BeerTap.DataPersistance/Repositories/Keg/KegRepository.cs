using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using BeerTap.DataPersistance.Entities;
using BeerTap.DomainServices.Keg;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common.Mapping;
using IQ.Platform.Messaging.Models;

namespace BeerTap.DataPersistance.Repositories.Keg
{
    public class KegRepository : IKegRepository
    {
        private readonly IDbContextFactory<BeerTapContext> _contextFactory;
        private readonly IMapper<KegRecord, KegDto> _dtoMapper;
        private readonly IMapper<KegDto, KegRecord> _recordMapper;

        private readonly Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public KegRepository(IDbContextFactory<BeerTapContext> contextFactory,
            IMapper<KegRecord, KegDto> dtoMapper,
            IMapper<KegDto, KegRecord> recordMapper)
        {
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));
            if (dtoMapper == null) throw new ArgumentNullException(nameof(dtoMapper));
            if (recordMapper == null) throw new ArgumentNullException(nameof(recordMapper));
            _contextFactory = contextFactory;
            _dtoMapper = dtoMapper;
            _recordMapper = recordMapper;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<KegDto> GetByIdAsync(int id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                var KegRecord = await context.Kegs.FindAsync(id).ConfigureAwait(false);
                return KegRecord == null
                           ? null
                           : _dtoMapper.Map(KegRecord);
            }
        }

        public async Task<KegDto> GetByTapIdAsync(int tapId)
        {
            using (var context = _contextFactory.CreateContext())
            {
                var query = from p in context.Kegs
                            where p.TapId == tapId
                            select p;

                if (! await query.AnyAsync().ConfigureAwait(false))
                    return null;

                var kegDto = _dtoMapper.Map(await query.FirstOrDefaultAsync().ConfigureAwait(false));
                return kegDto;
            }
        }

        public async Task<int> SaveNewAsync(KegDto kegDto)
        {
            try
            {
                var kegRecord = _recordMapper.Map(kegDto);

                using (var context = _contextFactory.CreateContext())
                {
                    context.Kegs.Add(kegRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    return kegRecord.Id;
                }
            }
            catch (DbUpdateException ex)
            {
                var exception = DbContextUtils.ConvertDbUpdateException(ex);
                Logger.Error(new ExpandableLogMessage(exception.ToString(), new KeyValuePair<string, object>("failureMessage", exception.ToString())));
                throw exception;
            }
        }

        public async Task UpdateAsync(KegDto kegDto)
        {
            try
            {
                using (var context = _contextFactory.CreateContext())
                {
                    var kegRecord = await context.Kegs.FindAsync(kegDto.Id).ConfigureAwait(false);

                    kegRecord.TapId = kegDto.TapId;
                    kegRecord.BeerName = kegDto.BeerName;
                    kegRecord.Volume = kegDto.Volume;
                    kegRecord.UpdatedByUserId = kegDto.UpdatedByUserId;
                    kegRecord.UpdatedDateUtc = kegDto.UpdatedDateUtc;

                    context.Kegs.Add(kegRecord);
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
                    var taxPartnerConfiguration = await context.Kegs.FindAsync(id).ConfigureAwait(false);

                    if (taxPartnerConfiguration == null)
                        return;

                    context.Kegs.Remove(taxPartnerConfiguration);
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
