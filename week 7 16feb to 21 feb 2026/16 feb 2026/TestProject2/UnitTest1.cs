using NUnit.Framework;
using System;

[TestFixture]
public class LibraryTests
{
    private Library library;

    [SetUp]
    public void Setup()
    {
        library = new Library();
        library.AddBook("C# Basics", 2);
    }

    [Test]
    public void AddBook_NewBook_IncreasesCount()
    {
        library.AddBook("ASP.NET Core", 3);

        Assert.That(library.GetBookCount("ASP.NET Core"), Is.EqualTo(3));
    }

    [Test]
    public void AddBook_ExistingBook_AddsQuantity()
    {
        library.AddBook("C# Basics", 3);

        Assert.That(library.GetBookCount("C# Basics"), Is.EqualTo(5));
    }

    [Test]
    public void BorrowBook_ExistingBook_DecreasesCount()
    {
        library.BorrowBook("C# Basics");

        Assert.That(library.GetBookCount("C# Basics"), Is.EqualTo(1));
    }

    [Test]
    public void BorrowBook_NonExistingBook_ThrowsException()
    {
        Assert.That(() => library.BorrowBook("Java"),
                    Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void BorrowBook_WhenQuantityBecomesZero_ThrowsException()
    {
        library.BorrowBook("C# Basics");
        library.BorrowBook("C# Basics");

        Assert.That(() => library.BorrowBook("C# Basics"),
                    Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void AddBook_WithInvalidTitle_ThrowsException()
    {
        Assert.That(() => library.AddBook("", 5),
                    Throws.TypeOf<ArgumentException>());
    }
}
