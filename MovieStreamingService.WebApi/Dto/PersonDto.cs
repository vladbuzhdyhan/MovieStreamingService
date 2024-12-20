﻿namespace MovieStreamingService.WebApi.Dto;

public class PersonDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public IFormFile? Image { get; set; }
    public string? Biography { get; set; }
    public string Slug { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaImage { get; set; }
    public List<MovieDto> Movies { get; set; } = [];
}