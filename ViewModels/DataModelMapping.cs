using AutoMapper;
using LiBaby.Models;

namespace LiBaby.ViewModels
{
	public class DataModelMapping : Profile
	{
		public DataModelMapping()
		{
			CreateMap<(Author author, Person person), AuthorViewModel>()
				.ForMember(d => d.AuthorId, a => a.MapFrom(s => s.author.AuthorId))
				.ForMember(d => d.Bio, a => a.MapFrom(s => s.author.Bio))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.person.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.person.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.person.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.person.RegistrationDate));

			CreateMap<(Reader reader, Person person), ReaderViewModel>()
				.ForMember(d => d.ReaderId, a => a.MapFrom(s => s.reader.ReaderId))
				.ForMember(d => d.Email, a => a.MapFrom(s => s.reader.Email))
				.ForMember(d => d.Address, a => a.MapFrom(s => s.reader.Address))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.person.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.person.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.person.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.person.RegistrationDate));

			CreateMap<(Employee employee, Person person), EmployeeViewModel>()
				.ForMember(d => d.EmployeeId, a => a.MapFrom(s => s.employee.EmployeeId))
				.ForMember(d => d.Salary, a => a.MapFrom(s => s.employee.Salary))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.person.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.person.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.person.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.person.RegistrationDate));

			CreateMap<AuthorViewModel, Author>()
				.ForMember(d => d.AuthorId, a => a.MapFrom(s => s.AuthorId))
				.ForMember(d => d.Bio, a => a.MapFrom(s => s.Bio));

			CreateMap<EmployeeViewModel, Employee>()
				.ForMember(d => d.EmployeeId, a => a.MapFrom(s => s.EmployeeId))
				.ForMember(d => d.Salary, a => a.MapFrom(s => s.Salary));

			CreateMap<ReaderViewModel, Reader>()
				.ForMember(d => d.ReaderId, a => a.MapFrom(s => s.ReaderId))
				.ForMember(d => d.Address, a => a.MapFrom(s => s.Address))
				.ForMember(d => d.Email, a => a.MapFrom(s => s.Email));

			CreateMap<AuthorViewModel, Person>()
				.ForMember(d => d.PersonId, a => a.MapFrom(s => s.AuthorId))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.RegistrationDate));

			CreateMap<EmployeeViewModel, Person>()
				.ForMember(d => d.PersonId, a => a.MapFrom(s => s.EmployeeId))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.RegistrationDate));

			CreateMap<ReaderViewModel, Person>()
				.ForMember(d => d.PersonId, a => a.MapFrom(s => s.ReaderId))
				.ForMember(d => d.FirstName, a => a.MapFrom(s => s.FirstName))
				.ForMember(d => d.LastName, a => a.MapFrom(s => s.LastName))
				.ForMember(d => d.Birthday, a => a.MapFrom(s => s.Birthday))
				.ForMember(d => d.RegistrationDate, a => a.MapFrom(s => s.RegistrationDate));

			CreateMap<PublisherViewModel, Publisher>();
			CreateMap<Publisher, PublisherViewModel>();
			CreateMap<BookViewModel, Book>();
			CreateMap<Book, BookViewModel>();
		}
	}
}