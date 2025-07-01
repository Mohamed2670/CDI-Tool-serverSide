using CDI_Tool.Dtos.LogDtos;
using CDI_Tool.Model;
using CDI_Tool.Repository;

namespace CDI_Tool.Service
{
    public class LogService(LogRepository _logRepository)
    {
        internal async Task<IEnumerable<Log>> GetAllLogsPaginated(int pageSize = 100, int pageNumber = 1)
        {
            return await _logRepository.GetAllLogsPaginated(pageSize, pageNumber);
        }
        internal async Task<LogResponseReadDto> GetFilterdLogsPaginated(RequestFilteredLogReadDto request, int pageSize = 100, int pageNumber = 1)
        {
            return await _logRepository.GetFilterdLogsPaginated(request, pageSize, pageNumber);
        }
        internal async Task<Log> CreateLog(LogCreateDto logCreateDto)
        {
            return await _logRepository.CreateLog(logCreateDto);
        }
    }
}