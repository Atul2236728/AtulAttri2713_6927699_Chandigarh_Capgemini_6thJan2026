using System;
using System.Collections.Generic;

public class Library
{
    private readonly Dictionary<string, int> books = new Dictionary<string, int>();

    public void AddBook(string title, int quantity)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        if (books.ContainsKey(title))
            books[title] += quantity;
        else
            books[title] = quantity;
    }

    public void BorrowBook(string title)
    {
        if (!books.ContainsKey(title))
            throw new InvalidOperationException("Book does not exist");

        if (books[title] == 0)
            throw new InvalidOperationException("Book not available");

        books[title]--;
    }

    public int GetBookCount(string title)
    {
        return books.ContainsKey(title) ? books[title] : 0;
    }
}
