using CDI_Tool.Data;
using CDI_Tool.Dtos.LogDtos;
using CDI_Tool.Model;
using Microsoft.EntityFrameworkCore;


namespace CDI_Tool.Repository
{
    public class LogRepository : GenericRepository<Log>
    {
        private readonly CDIDBContext _context;
        public LogRepository(CDIDBContext context) : base(context)
        {
            _context = context;
        }
        internal async Task<IEnumerable<Log>> GetAllLogsPaginated(int pageSize = 100, int pageNumber = 1)
        {
            return await _context.Logs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        internal async Task<Log> CreateLog(LogCreateDto logCreateDto)
        {
            var log = new Log
            {
                GuestName = logCreateDto.GuestName,
                LastName = logCreateDto.LastName,
                DOB = DateTime.Parse(logCreateDto.DOB),
                FirstInitial = logCreateDto.FirstInitial,
                MRN = logCreateDto.MRN,
                Insurance = logCreateDto.Insurance,
                Drugs = logCreateDto.SelectedDrugs,
                TotalProfit = decimal.Parse(logCreateDto.TotalProfit),
                Decision = logCreateDto.Decision,
                TransactionID = logCreateDto.TransactionID,
                Timestamp = logCreateDto.Timestamp,
            };
            var entry = await _context.Logs.AddAsync(log);
            return entry.Entity;
        }
        internal async Task<LogResponseReadDto> GetFilterdLogsPaginated(RequestFilteredLogReadDto request, int pageSize = 100, int pageNumber = 1)
        {
            var filtered = _context.Logs.AsQueryable();

            if (!string.IsNullOrEmpty(request.guestName))
                filtered = filtered.Where(l => l.GuestName != null && l.GuestName.Contains(request.guestName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(request.decision))
                filtered = filtered.Where(l => l.Decision == $"Send to {request.decision}");

            if (request.dateFrom.HasValue)
            {
                var from = request.dateFrom.Value;
                filtered = filtered.Where(l => DateTime.Parse(l.Timestamp) >= from);
            }

            if (request.dateTo.HasValue)
            {
                var to = request.dateTo.Value;
                filtered = filtered.Where(l => DateTime.Parse(l.Timestamp) <= to);
            }

            var total = await filtered.CountAsync();

            var paginated = await filtered
                .OrderByDescending(l => DateTime.Parse(l.Timestamp))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var logsList = await filtered.ToListAsync(); // only once for analytics

            var analytics = CalculateAnalytics(logsList);

            return new LogResponseReadDto
            {
                Data = paginated,
                Total = total,
                Analytics = analytics
            };
        }

        private AnalyticsInfo CalculateAnalytics(List<Log> logs)
        {
            var total = logs.Count;
            var apple = logs.Count(l => l.Decision == "Send to Apple");
            var grand = logs.Count(l => l.Decision == "Send to Grand");

            var userStats = logs
                .GroupBy(l => l.GuestName)
                .Select(g => new TopUser
                {
                    Name = g.Key ?? "Unknown",
                    Count = g.Count(),
                    TotalProfit = g.Sum(x => x.TotalProfit)
                })
                .OrderByDescending(u => u.Count)
                .Take(5)
                .ToList();

            return new AnalyticsInfo
            {
                TotalDecisions = total,
                AppleDecisions = apple,
                GrandDecisions = grand,
                ApplePercentage = total > 0 ? (int)Math.Round((double)apple / total * 100) : 0,
                GrandPercentage = total > 0 ? (int)Math.Round((double)grand / total * 100) : 0,
                TopUsers = userStats
            };
        }
    }
}