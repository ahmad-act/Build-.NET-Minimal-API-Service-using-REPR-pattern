namespace BookInformationService.BookInformation.Facade.Create
{
    public static class CreateBookInformationExtension
    {
        public static BookInformationModel ToBookInformationModel(this BookInformation.Create.CreateRequest request)
        {
            if (request is null)
            {
                return null;
            }

            return new BookInformationModel
            {
                Id = 0,
                Title = request.Title,
                Stock = request.Stock,
                Available = request.Stock
            };
        }
    }
}
