using AutoMapper;
using StARKS.Application.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace StARKS.Application.Test.Services
{
    public class AutoMapperFixture 
    {
        public IMapper Instance { get; set; }

        public AutoMapperFixture()
        {
            var myProfile = new OrganizationProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            Instance = new Mapper(configuration);
        }
    }
}
