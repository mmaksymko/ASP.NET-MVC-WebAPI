using Microsoft.EntityFrameworkCore;

namespace LiBaby.Models;

public partial class KpzDbContext : DbContext
{
	public KpzDbContext()
	{
	}

	public KpzDbContext(DbContextOptions<KpzDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Author> Authors { get; set; }

	public virtual DbSet<AvailableBook> AvailableBooks { get; set; }

	public virtual DbSet<Book> Books { get; set; }

	public virtual DbSet<BookAuthor> BookAuthors { get; set; }

	public virtual DbSet<BookCopy> BookCopies { get; set; }

	public virtual DbSet<BookGenre> BookGenres { get; set; }

	public virtual DbSet<BookLanguage> BookLanguages { get; set; }

	public virtual DbSet<BookSeries> BookSeries { get; set; }

	public virtual DbSet<Borrow> Borrows { get; set; }

	public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

	public virtual DbSet<Employee> Employees { get; set; }

	public virtual DbSet<Genre> Genres { get; set; }

	public virtual DbSet<Language> Languages { get; set; }

	public virtual DbSet<Person> People { get; set; }

	public virtual DbSet<Publisher> Publishers { get; set; }

	public virtual DbSet<Reader> Readers { get; set; }

	public virtual DbSet<ReturnedBorrow> ReturnedBorrows { get; set; }

	public virtual DbSet<Series> Series { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.UseCollation("utf8mb4_0900_ai_ci")
			.HasCharSet("utf8mb4");

		modelBuilder.Entity<Author>(entity =>
		{
			entity.HasKey(e => e.AuthorId).HasName("PRIMARY");

			entity.ToTable("author");

			entity.Property(e => e.AuthorId).HasColumnName("author_id");
			entity.Property(e => e.Bio)
				.HasColumnType("text")
				.HasColumnName("bio");
		});

		modelBuilder.Entity<AvailableBook>(entity =>
		{
			entity
				.HasNoKey()
				.ToView("available_book");

			entity.Property(e => e.BookCondition)
				.HasColumnType("enum('perfect','good','average','bad')")
				.HasColumnName("book_condition");
			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
			entity.Property(e => e.Pages).HasColumnName("pages");
			entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.HasColumnName("title");
		});

		modelBuilder.Entity<Book>(entity =>
		{
			entity.HasKey(e => e.BookId).HasName("PRIMARY");

			entity.ToTable("book");

			entity.HasIndex(e => e.PublisherId, "publisher_id");

			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.BookAdded)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp")
				.HasColumnName("book_added");
			entity.Property(e => e.Pages).HasColumnName("pages");
			entity.Property(e => e.PublisherId)
				.HasDefaultValueSql("'1'")
				.HasColumnName("publisher_id");
			entity.Property(e => e.ReleaseYear).HasColumnName("release_year");
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.HasColumnName("title");
		});

		modelBuilder.Entity<BookAuthor>(entity =>
		{
			entity.HasKey(e => new { e.BookId, e.AuthorId })
				.HasName("PRIMARY")
				.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

			entity.ToTable("book_author");

			entity.HasIndex(e => e.AuthorId, "author_id");

			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.AuthorId).HasColumnName("author_id");
		});

		modelBuilder.Entity<BookCopy>(entity =>
		{
			entity.HasKey(e => e.InventoryId).HasName("PRIMARY");

			entity.ToTable("book_copy");

			entity.HasIndex(e => e.BookId, "book_id");

			entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
			entity.Property(e => e.BookCondition)
				.HasColumnType("enum('perfect','good','average','bad')")
				.HasColumnName("book_condition");
			entity.Property(e => e.BookId).HasColumnName("book_id");
		});

		modelBuilder.Entity<BookGenre>(entity =>
		{
			entity.HasKey(e => new { e.BookId, e.GenreId })
				.HasName("PRIMARY")
				.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

			entity.ToTable("book_genre");

			entity.HasIndex(e => e.GenreId, "genre_id");

			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.GenreId).HasColumnName("genre_id");
		});

		modelBuilder.Entity<BookLanguage>(entity =>
		{
			entity.HasKey(e => new { e.BookId, e.LanguageId })
				.HasName("PRIMARY")
				.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

			entity.ToTable("book_language");

			entity.HasIndex(e => e.LanguageId, "language_id");

			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.LanguageId).HasColumnName("language_id");
		});

		modelBuilder.Entity<BookSeries>(entity =>
		{
			entity.HasKey(e => new { e.BookId, e.SeriesId })
				.HasName("PRIMARY")
				.HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

			entity.ToTable("book_series");

			entity.HasIndex(e => e.SeriesId, "series_id");

			entity.Property(e => e.BookId).HasColumnName("book_id");
			entity.Property(e => e.SeriesId).HasColumnName("series_id");
		});

		modelBuilder.Entity<Borrow>(entity =>
		{
			entity.HasKey(e => e.BorrowId).HasName("PRIMARY");

			entity.ToTable("borrow");

			entity.HasIndex(e => e.EmployeeId, "employee_id");

			entity.HasIndex(e => e.InventoryId, "inventory_id");

			entity.HasIndex(e => e.ReaderId, "reader_id");

			entity.Property(e => e.BorrowId).HasColumnName("borrow_id");
			entity.Property(e => e.DueDate)
				.HasDefaultValueSql("(`issue_date` + interval 2 week)")
				.HasColumnName("due_date");
			entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
			entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
			entity.Property(e => e.IssueDate)
				.HasDefaultValueSql("cast(now() as date)")
				.HasColumnName("issue_date");
			entity.Property(e => e.ReaderId).HasColumnName("reader_id");
		});

		modelBuilder.Entity<Efmigrationshistory>(entity =>
		{
			entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

			entity.ToTable("__efmigrationshistory");

			entity.Property(e => e.MigrationId).HasMaxLength(150);
			entity.Property(e => e.ProductVersion).HasMaxLength(32);
		});

		modelBuilder.Entity<Employee>(entity =>
		{
			entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

			entity.ToTable("employee");

			entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
			entity.Property(e => e.Salary).HasColumnName("salary");
		});

		modelBuilder.Entity<Genre>(entity =>
		{
			entity.HasKey(e => e.GenreId).HasName("PRIMARY");

			entity.ToTable("genre");

			entity.Property(e => e.GenreId).HasColumnName("genre_id");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
		});

		modelBuilder.Entity<Language>(entity =>
		{
			entity.HasKey(e => e.LanguageId).HasName("PRIMARY");

			entity.ToTable("language");

			entity.Property(e => e.LanguageId).HasColumnName("language_id");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
			entity.Property(e => e.NativeName)
				.HasMaxLength(255)
				.HasColumnName("native_name");
		});

		modelBuilder.Entity<Person>(entity =>
		{
			entity.HasKey(e => e.PersonId).HasName("PRIMARY");

			entity.ToTable("person");

			entity.Property(e => e.PersonId).HasColumnName("person_id");
			entity.Property(e => e.Birthday).HasColumnName("birthday");
			entity.Property(e => e.FirstName)
				.HasMaxLength(60)
				.HasColumnName("first_name");
			entity.Property(e => e.LastName)
				.HasMaxLength(60)
				.HasColumnName("last_name");
			entity.Property(e => e.RegistrationDate)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp")
				.HasColumnName("registration_date");
		});

		modelBuilder.Entity<Publisher>(entity =>
		{
			entity.HasKey(e => e.PublisherId).HasName("PRIMARY");

			entity.ToTable("publisher");

			entity.HasIndex(e => e.Name, "name").IsUnique();

			entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
			entity.Property(e => e.Country)
				.HasMaxLength(255)
				.HasDefaultValueSql("'Not Specified'")
				.HasColumnName("country");
			entity.Property(e => e.Name).HasColumnName("name");
			entity.Property(e => e.PublisherAdded)
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.HasColumnType("timestamp")
				.HasColumnName("publisher_added");
		});

		modelBuilder.Entity<Reader>(entity =>
		{
			entity.HasKey(e => e.ReaderId).HasName("PRIMARY");

			entity.ToTable("reader");

			entity.Property(e => e.ReaderId).HasColumnName("reader_id");
			entity.Property(e => e.Address)
				.HasMaxLength(255)
				.HasDefaultValueSql("'Not Specified'")
				.HasColumnName("address");
			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.HasColumnName("email");
		});

		modelBuilder.Entity<ReturnedBorrow>(entity =>
		{
			entity.HasKey(e => e.BorrowId).HasName("PRIMARY");

			entity.ToTable("returned_borrow");

			entity.Property(e => e.BorrowId)
				.ValueGeneratedNever()
				.HasColumnName("borrow_id");
			entity.Property(e => e.ReturnDate)
				.HasDefaultValueSql("cast(now() as date)")
				.HasColumnName("return_date");
		});

		modelBuilder.Entity<Series>(entity =>
		{
			entity.HasKey(e => e.SeriesId).HasName("PRIMARY");

			entity.ToTable("series");

			entity.Property(e => e.SeriesId).HasColumnName("series_id");
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.HasColumnName("name");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
