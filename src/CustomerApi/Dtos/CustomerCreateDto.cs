namespace CustomerApi.Dtos;

using System.ComponentModel.DataAnnotations;


public class CustomerCreateDto  {

    [MinLength(1)][MaxLength(100)]
    public string Name {get; set;  } = string.Empty; 
    [MinLength(1)][EmailAddress]
    public string Email {get; set; } = string.Empty;
    public string? Phone {get; set; }
    public string? Company{get; set; }

}