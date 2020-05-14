//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Birder.Services
//{
//    public class ListBooksService
//    {
//        private readonly EfCoreContext _context;

//        public ListBooksService(EfCoreContext context)
//        {
//            _context = context;
//        }

//        public IQueryable<BookListDto> SortFilterPage
//            (SortFilterPageOptions options)
//        {
//            var booksQuery = _context.Books
//                .AsNoTracking()
//                .MapBookToDto()
//                .OrderBooksBy(options.OrderByOptions)
//                .FilterBooksBy(options.FilterBy,
//                               options.FilterValue);
//            options.SetupRestOfDto(booksQuery);
//            return booksQuery.Page(options.PageNum - 1,
//                                   options.PageSize);
//        }
//    }



//}
