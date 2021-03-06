namespace LibrairiePoc.Infra.Tests;

using FluentAssertions;
using LibrairiePoc.Infra.Ports.Controller;
using LibrairiePoc.UsesCase.Builder;
using LibrairiePoc.UsesCase.Entities;
using LibrairiePoc.UsesCase.Ports.Storages;
using LibrairiePoc.UsesCase.Request;
using LibrairiePoc.UsesCase.Tools;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

public class BookStorageEFShould
{
    public IBookStorage bookRepository;

    public DbContext Context { get; }

    public BookStorageEFShould(BookRepositoryEF bookEf, DbContext context)
    {
        this.bookRepository = bookEf;
        this.Context = context;
    }

    [Fact]
    public void BookRepository_GetMany_ShouldReturn_PaginedBook()
    {

        var assert = bookRepository.GetMany(new GetBooksRequest());

        assert.Should().BeEquivalentTo(
            new PaginedData<Book>()
            {
                Page = 1,
                PageSize = 20,
                Data = new[]
                {
                    new BookBuilder("CCIsbn")
                        .Title( "Clean Code")
                        .Autor( "Robert C Martin")
                        .Category( "technique")
                        .Price( 25.80m)
                        .Build()  ,
                    new BookBuilder("CDRIsbn")
                        .Title("Le cycle des robots")
                        .Autor("Isaac Asimov")
                        .Category("SF")
                        .Price(6m)
                        .Build()
                }
            });
    }

    [Fact]
    public void BookRepository_GetMany_WithPageSizeAt1AndPageNumberAt2_ShouldReturnCorectPagination()
    {
        var assert = bookRepository.GetMany(new GetBooksRequest() { PageSize = 1, PageNumber = 2 });
        assert.Should().BeEquivalentTo(
            new PaginedData<Book>()
            {
                Page = 2,
                PageSize = 1,
                Data = new[]
                {
                    new BookBuilder("CDRIsbn")
                        .Title("Le cycle des robots")
                        .Autor("Isaac Asimov")
                        .Category("SF")
                        .Price(6m)
                        .Build()
                }
            });
    }

    [Fact]
    public void BookRepository_GetMany_WithPageSizeAt5AndPageNumberAt2_ShouldReturnCorectPagination()
    {
        var assert = bookRepository.GetMany(new GetBooksRequest() { PageSize = 3, PageNumber = 5 });
        assert.Should().BeEquivalentTo(
            new PaginedData<Book>()
            {
                Page = 5,
                PageSize = 3,
                Data = new Book[0],
            });
    }
}
