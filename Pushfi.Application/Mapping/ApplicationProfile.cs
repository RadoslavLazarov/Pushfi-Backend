using AutoMapper;
using Pushfi.Application.Broker.Commands;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Common.Models.Broker;
using Pushfi.Application.Common.Models.Enfortra;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Entities.Email;
using System.Globalization;

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

            this.CreateMap<ApplicationUser, BrokerRegistrationCommand>()
                .ForMember(x => x.Email, y => y.MapFrom(src => src.UserName))
                .ReverseMap();

            this.CreateMap<BrokerEntity, BrokerRegistrationCommand>()
                .ReverseMap();

            this.CreateMap<BrokerEntity, BrokerModel>()
                .ForMember(x => x.UserId, y => y.MapFrom(src => src.User.Id))
                .ForMember(x => x.Email, y => y.MapFrom(src => src.User.Email))
                .ReverseMap();

            this.CreateMap<EmailTemplateEntity, EmailModel>();

            this.CreateMap<CustomerEmailHistoryEntity, CustomerEmailHistoryModel>()
                .ReverseMap();

            this.CreateMap<CustomerEmailHistoryEntity, LatestOfferResponseModel>()
                .ForMember(x => x.LowOffer, y => y.MapFrom(src => String.Format(CultureInfo.InvariantCulture, "{0:N0}", src.LowOffer)))
                .ForMember(x => x.HighOffer, y => y.MapFrom(src => String.Format(CultureInfo.InvariantCulture, "{0:N0}", src.HighOffer)))
                .ReverseMap();

            this.CreateMap<BrokerEntity, BrokerDataForCustomerFormModel>()
                .ForPath(x => x.LogoImageUrl, y => y.MapFrom(src => src.LogoImage.Url));
        }
    }
}
