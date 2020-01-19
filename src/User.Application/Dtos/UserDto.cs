using BAOZ.Common.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace User.Application.Dtos
{
    public class UserDto : IDto
    {
        public UserDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
