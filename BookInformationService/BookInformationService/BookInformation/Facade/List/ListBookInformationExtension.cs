namespace BookInformationService.BookInformation.Facade.List
{
    public static class ListBookInformationExtension
    {
        // Asynchronous conversion from List<BookInformationModel> to List<BookInformationDto> using PLINQ
        public static async Task<List<ListBookInformationDto>> ToListBookInformationDtoListAsync(this List<BookInformationModel> bookInformationModelList)
        {
            if (bookInformationModelList == null || bookInformationModelList.Count == 0)
            {
                return new List<ListBookInformationDto>();
            }

            // Use Task.Run to ensure the PLINQ operation is run asynchronously
            return await Task.Run(() =>
            {
                // Use PLINQ to process the list in parallel
                return bookInformationModelList
                    .AsParallel()
                    .AsOrdered() // Ensure the results preserve the order of the source list
                    .Select(model => new ListBookInformationDto
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Stock = model.Stock,
                        Available = model.Available
                    })
                    .ToList();
            });
        }
    }
}
