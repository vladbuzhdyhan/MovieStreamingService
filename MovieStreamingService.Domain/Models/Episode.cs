﻿using MovieStreamingService.Domain.Common;

namespace MovieStreamingService.Domain.Models;

public class Episode : AuditableSeoEntity<int>
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int SeasonId { get; set; }
    public int Duration { get; set; }
    public DateTime AirDate { get; set; }
    public string Video { get; set; }
    public List<Comment> Comments { get; set; } = [];
    public Season Season { get; set; }
}