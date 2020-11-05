﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CloudServise_API.Models
{
    public class Requirement
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        
        public Guid LaboratoryWorkId { get; set; }
        [ForeignKey("LaboratoryWorkId")]
        public LaboratoryWork LaboratoryWork { get; set; }

        public List<File> Files { get; set; }

        public Requirement() {}
        public Requirement(string description, Guid laboratoryWorkId, List<File> files)
        {
            Description = description;
            LaboratoryWorkId = laboratoryWorkId;
            Files.AddRange(Files);
        }

        public RequirementDTO ToRequirementDto()
        {
            return new RequirementDTO(Id, Description, LaboratoryWorkId, Files);
        }
    }

    public class RequirementDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid LaboratoryWorkId { get; set; }
        public List<FileDTO> Files { get; set; }

        public RequirementDTO() {}
        public RequirementDTO(Guid id, string description, Guid laboratoryWorkId, List<File> files)
        {
            Id = id;
            Description = description;
            LaboratoryWorkId = laboratoryWorkId;
            foreach (var item in files)
            {
                Files.Add(item.ToFileDto());
            }
        }
    }
}
