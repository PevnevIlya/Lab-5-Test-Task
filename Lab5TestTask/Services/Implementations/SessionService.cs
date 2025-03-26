using Lab5TestTask.Data;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lab5TestTask.Services.Implementations;

/// <summary>
/// SessionService implementation.
/// Implement methods here.
/// </summary>
public class SessionService : ISessionService
{
    private readonly ApplicationDbContext _dbContext;

    public SessionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Session> GetSessionAsync()
    {
        return await _dbContext.Sessions
            .Where(s => s.DeviceType == "Desktop")
            .OrderBy(s => s.StartTime)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Session>> GetSessionsAsync()
    {
        return await _dbContext.Sessions
            .Join(_dbContext.Users,
                  session => session.UserId,
                  user => user.Id,
                  (session, user) => new { Session = session, User = user })
            .Where(x => x.User.IsActive
                     && x.Session.EndTime != null
                     && x.Session.EndTime < new DateTime(2025, 1, 1))
            .Select(x => x.Session)
            .ToListAsync();
    }
}