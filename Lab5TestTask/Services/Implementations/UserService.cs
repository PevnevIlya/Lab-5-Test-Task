using Lab5TestTask.Data;
using Lab5TestTask.Models;
using Lab5TestTask.Services.Interfaces;

public class UserService : IUserService
{
	private readonly ApplicationDbContext _dbContext;

	public UserService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<User> GetUserAsync()
	{
		return await _dbContext.Users
			.Select(u => new
			{
				User = u,
				SessionCount = _dbContext.Sessions.Count(s => s.UserId == u.Id)
			})
			.OrderByDescending(x => x.SessionCount)
			.Select(x => x.User)
			.FirstOrDefaultAsync();
	}

	public async Task<List<User>> GetUsersAsync()
	{
		return await _dbContext.Users
			.Where(u => _dbContext.Sessions
				.Any(s => s.UserId == u.Id && s.DeviceType == "Mobile"))
			.ToListAsync();
	}
}