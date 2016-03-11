using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using BeerTap.DataPersistance.Entities;
using BeerTap.DomainServices.Office;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common.Mapping;
using IQ.Platform.Messaging.Models;

namespace BeerTap.DataPersistance.Repositories.Office
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IDbContextFactory<BeerTapContext> _contextFactory;
        private readonly IMapper<OfficeRecord, OfficeDto> _dtoMapper;
        private readonly IMapper<OfficeDto, OfficeRecord> _recordMapper;

        private readonly Lazy<ILog> _lazyLogger;

        ILog Logger
        {
            get { return _lazyLogger.Value; }
        }

        public OfficeRepository(IDbContextFactory<BeerTapContext> contextFactory,
            IMapper<OfficeRecord, OfficeDto> dtoMapper,
            IMapper<OfficeDto, OfficeRecord> recordMapper)
        {
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));
            if (dtoMapper == null) throw new ArgumentNullException(nameof(dtoMapper));
            if (recordMapper == null) throw new ArgumentNullException(nameof(recordMapper));
            _contextFactory = contextFactory;
            _dtoMapper = dtoMapper;
            _recordMapper = recordMapper;

            _lazyLogger = new Lazy<ILog>(LogManager.GetCurrentClassLogger);
        }

        public async Task<OfficeDto> GetByIdAsync(int id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                var officeRecord = await context.Offices.FindAsync(id).ConfigureAwait(false);
                return officeRecord == null
                           ? null
                           : _dtoMapper.Map(officeRecord);
            }
        }

        public async Task<IEnumerable<OfficeDto>> GetByNameAsync(string name)
        {
            var officeDtoRecords = new List<OfficeDto>();

            using (var context = _contextFactory.CreateContext())
            {
                var query = from p in context.Offices
                            where p.Name.Contains(name) 
                            select p;

                await query.ForEachAsync(x => officeDtoRecords.Add(_dtoMapper.Map(x)));
                return officeDtoRecords;
            }
        }

        public async Task<IEnumerable<OfficeDto>> GetAllAsync()
        {
            var officeDtoRecords = new List<OfficeDto>();

            using (var context = _contextFactory.CreateContext())
            {
                var query = from p in context.Offices
                            select p;

                await query.ForEachAsync(x => officeDtoRecords.Add(_dtoMapper.Map(x)));
                return officeDtoRecords;
            }
        }

        public async Task<int> SaveNewAsync(OfficeDto officeDto)
        {
            try
            {
                var officeRecord = _recordMapper.Map(officeDto);

                using (var context = _contextFactory.CreateContext())
                {
                    context.Offices.Add(officeRecord);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    return officeRecord.Id;
                }
            }
            catch (DbUpdateException ex)
            {
                var exception = DbContextUtils.ConvertDbUpdateException(ex);
                Logger.Error(new ExpandableLogMessage(exception.ToString(), new KeyValuePair<string, object>("failureMessage", exception.ToString())));
                throw exception;
            }
        }

        public async Task UpdateAsync(OfficeDto officeDto)
        {
            try
            {
                using (var context = _contextFactory.CreateContext())
                {
                    var taxPartnerConfiguration = await context.Offices.FindAsync(officeDto.Id).ConfigureAwait(false);

                    if (taxPartnerConfiguration != null)
                    {
                        taxPartnerConfiguration.Name = officeDto.Name;
                        taxPartnerConfiguration.UpdatedByUserId = officeDto.UpdatedByUserId;
                        taxPartnerConfiguration.UpdatedDateUtc = officeDto.UpdatedDateUtc;
                    }

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
                    var taxPartnerConfiguration = await context.Offices.FindAsync(id).ConfigureAwait(false);

                    if (taxPartnerConfiguration == null)
                        return;

                    context.Offices.Remove(taxPartnerConfiguration);
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
