﻿namespace BookStore.Models.Dtos;

public static class PagingExtensions
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> queryable, BasePaging pager)
    {
        return queryable.Skip(pager.SkipEntity).Take(pager.TakeEntity);
    }
}
