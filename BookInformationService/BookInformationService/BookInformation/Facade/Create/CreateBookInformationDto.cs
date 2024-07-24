using System.ComponentModel.DataAnnotations;

namespace BookInformationService.BookInformation.Facade.Create;

public class CreateBookInformationDto
{
    /// <summary>
    /// Gets or sets the title of the book information.
    /// </summary>
    /// <example>The Great Gatsby</example>
    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the stock of the book information.
    /// </summary>
    /// <example>5</example>
    public int Stock { get; set; }

    // Explicit conversion from CreateBookInformationDto to BookInformationModel
    public static explicit operator BookInformationModel(CreateBookInformationDto createBookInformationDto)
    {
        if (createBookInformationDto is null)
        {
            return null;
        }

        return new BookInformationModel
        {
            Id = 0,
            Title = createBookInformationDto.Title,
            Stock = createBookInformationDto.Stock,
            Available = createBookInformationDto.Stock
        };
    }
}

