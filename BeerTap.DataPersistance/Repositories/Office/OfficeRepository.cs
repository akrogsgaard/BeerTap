using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BeerTap.DataPersistance.Entities;
using BeerTap.DomainServices.Office;
using BeerTap.Transport;
using IQ.Foundation.Logging;
using IQ.Platform.Framework.Common.Mapping;

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
    }
}
