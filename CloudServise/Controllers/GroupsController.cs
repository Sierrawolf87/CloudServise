﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudService_API.Data;
using CloudService_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudService_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(ApplicationDbContext context, ILogger<GroupsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Groups
        [Authorize(Roles = "root, admin, network_editor")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups()
        {
            var find = await _context.Groups.ToListAsync();
            List<GroupDTO> groupDtos = new List<GroupDTO>();
            foreach (var item in find)
            {
                groupDtos.Add(item.ToGroupDto());
            }

            return Ok(groupDtos);
        }

        // GET: api/Groups/5
        [Authorize(Roles = "root, admin, network_editor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDTO>> GetGroup(Guid id)
        {
            var find = await _context.Groups.FindAsync(id);

            if (find == null)
            {
                return NotFound();
            }

            return find.ToGroupDto();
        }

        [Authorize]
        [HttpGet("{id}/GetDisciplines")]
        public async Task<List<DisciplineDTO>> GetDiscipline(Guid id)
        {
            var discipline = await _context.Disciplines.Include(c => c.DisciplineGroupTeachers.Where(v => v.GroupId == id)).ToListAsync();
            List<DisciplineDTO> disciplineDtos = new List<DisciplineDTO>();
            foreach (var item in discipline)
            {
                disciplineDtos.Add(item.ToDisciplineDto());
            }

            return disciplineDtos;
        }

        // PUT: api/Groups/5
        [Authorize(Roles = "root, admin, network_editor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(Guid id, GroupDTO groupDto)
        {
            if (id != groupDto.Id)
            {
                return BadRequest();
            }

            var find = await _context.Groups.FindAsync(id);
            _context.Entry(groupDto).State = EntityState.Modified;
            find.Name = groupDto.Name;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        [Authorize(Roles = "root, admin, network_editor")]
        [HttpPost]
        public async Task<ActionResult<GroupDTO>> PostGroup(GroupDTO groupDto)
        {
            Group group = new Group(groupDto.Name);
            try
            {
                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();
                return Created("", group.ToGroupDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // DELETE: api/Groups/5
        [Authorize(Roles = "root, admin, network_editor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupDTO>> DeleteGroup(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return group.ToGroupDto();
        }

        private bool GroupExists(Guid id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
