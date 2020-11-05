﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CloudServise_API.Models
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(5)]
        public string Name { get; set; }

        public List<DisciplineGroupTeacher> DisciplineGroupTeachers { get; set; }

        public Group() {}
        public Group(string name)
        {
            Name = name;
        }

        public GroupDTO ToGroupDto()
        {
            GroupDTO groupDto = new GroupDTO(Id, Name);
            return groupDto;
        }
    }

    public class GroupDTO
    {
        public Guid Id;
        public string Name;

        public GroupDTO() {}
        public GroupDTO(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
