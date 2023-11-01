using System;
using Microsoft.EntityFrameworkCore;

namespace web_bmstu.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<RecordingStudio> RecordingStudios { get; set; }
        public DbSet<SongPlaylist> SongPlaylists { get; set; }

        // Дополнительные настройки и методы контекста данных
    }
}