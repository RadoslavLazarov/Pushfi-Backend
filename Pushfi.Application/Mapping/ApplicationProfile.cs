using AutoMapper;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Common.Models.Enfortra;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Entities.Email;

namespace Pushfi.Application.Mapping
{
	public class ApplicationProfile : Profile
	{

        public ApplicationProfile()
        {
            this.CreateMap<ApplicationUser, UserModel>()
                .ForMember(x => x.Email, y => y.MapFrom(src => src.UserName))
                .ReverseMap();

            this.CreateMap<ApplicationUser, RegistrationCommand>()
                .ForMember(x => x.Email, y => y.MapFrom(src => src.UserName))
                .ReverseMap();

            this.CreateMap<CustomerEntity, RegistrationCommand>()
                .ReverseMap();

            this.CreateMap<CustomerEntity, CustomerModel>()
                .ForMember(x => x.UserId, y => y.MapFrom(src => src.User.Id))
                .ForMember(x => x.Email, y => y.MapFrom(src => src.User.Email))
                .ForMember(x => x.IsDeleted, y => y.MapFrom(src => src.User.IsDeleted))
                .ReverseMap();

            this.CreateMap<RegistrationCommand, CreateNewUserEnrollmentModel>()
                .ForMember(x => x.DOB, y => y.MapFrom(src => src.DateOfBirth))
                .ForMember(x => x.Address, y => y.MapFrom(src => src.StreetAddress))
                .ForMember(x => x.State, y => y.MapFrom(src => src.Region))
                .ForMember(x => x.ZipCode, y => y.MapFrom(src => src.PostalCode))
                .ForMember(x => x.SMSPhone, y => y.MapFrom(src => src.MobilePhoneNumber))
                .ForMember(x => x.SMSPhoneCarrier, y => y.MapFrom(src => src.SMSphoneCarrier));

            this.CreateMap<EmailTemplateEntity, EmailModel>();

            this.CreateMap<CustomerEmailHistoryEntity, CustomerEmailHistoryModel>()
                .ReverseMap();
        }
    }
}
