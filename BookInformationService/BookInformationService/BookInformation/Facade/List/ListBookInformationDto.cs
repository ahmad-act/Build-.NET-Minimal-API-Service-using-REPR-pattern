using System.ComponentModel.DataAnnotations;

namespace BookInformationService.BookInformation.Facade.List;

public class ListBookInformationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the book information.
    /// </summary>
    /// <example>1</example>
    [Required]
    [Key]
    public int Id { get; set; }

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
    [Required]
    [Range(0, 100)]
    public int Stock { get; set; }

    /// <summary>
    /// Gets or sets the available stock of the book information.
    /// </summary>
    /// <example>5</example>
    [Required]
    [Range(0, 100)]
    public int Available { get; set; }

    // Implicit conversion from BookInformationModel to BookInformationDto
    public static implicit operator ListBookInformationDto(BookInformationModel bookInformationModel)
    {
        if (bookInformationModel is null)
        {
            return null;
        }

        return new ListBookInformationDto
        {
            Id = bookInformationModel.Id,
            Title = bookInformationModel.Title,
            Stock = bookInformationModel.Stock,
            Available = bookInformationModel.Available
        };
    }
}
