﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Musica_BauteViviana.Models
{
    [Table("Playlist")]
    public partial class Playlist
    {
        public Playlist()
        {
            Tracks = new HashSet<Track>();
        }

        [Key]
        public int PlaylistId { get; set; }
        [StringLength(120)]
        public string Name { get; set; }

        [ForeignKey("PlaylistId")]
        [InverseProperty("Playlists")]
        public virtual ICollection<Track> Tracks { get; set; }
    }
}