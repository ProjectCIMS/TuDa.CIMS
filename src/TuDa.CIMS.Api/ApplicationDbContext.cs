using Microsoft.EntityFrameworkCore;

namespace TuDa.CIMS.Api;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options) { }
