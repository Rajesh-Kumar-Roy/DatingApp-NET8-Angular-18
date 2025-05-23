﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entites
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; }
        public bool IsApproved { get; set; }
        public string? PublicId { get; set; }

        //Navigation properties
        // required one to many relationship

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}