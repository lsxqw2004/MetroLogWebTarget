using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MetroLogWebTarget.Core.Infrastructure;
using MetroLogWebTarget.Domain;
using MetroLogWebTarget.Web.Models;

namespace MetroLogWebTarget.Web.Framework
{
    public class AutoMapperStartupTask: IStartupTask
    {
        public void Execute()
        {
            ////role
            //Mapper.CreateMap<Role, RoleModel>();

            //Mapper.CreateMap<RoleModel, Role>()
            //.ForMember(dest => dest.PermissionRecords, mo => mo.Ignore());

            ////user
            //Mapper.CreateMap<User, UserModel>()
            //    .ForMember(dest => dest.UserRoles, mo => mo.MapFrom(src => src.UserRoles.Select(ur => ur.Name).Join(",")))
            //    .ForMember(dest => dest.SelectedUserRoleIds, mo => mo.Ignore())
            //    .ForMember(dest => dest.AvailableRoles, mo => mo.Ignore());

            //Mapper.CreateMap<UserModel, User>();

            //role
            Mapper.CreateMap<LogEnvironment, LogEnvironmentModel>();

            Mapper.CreateMap<LogEnvironmentModel, LogEnvironment>();
            //.ForMember(dest => dest.PermissionRecords, mo => mo.Ignore());

            //user
            Mapper.CreateMap<LogEvent, LogEventModel>();
                //.ForMember(dest => dest.UserRoles, mo => mo.MapFrom(src => src.UserRoles.Select(ur => ur.Name).Join(",")))
                //.ForMember(dest => dest.SelectedUserRoleIds, mo => mo.Ignore())
                //.ForMember(dest => dest.AvailableRoles, mo => mo.Ignore());

            Mapper.CreateMap<LogEventModel, LogEvent>()
                .ForMember(dest=>dest.LogEnvironmentId,mo=>mo.Ignore());
        }

        public int Order { get { return 0; } }
    }
}