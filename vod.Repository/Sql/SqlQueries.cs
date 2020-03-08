namespace vod.Repository.Sql
{
    public static class SqlQueries
    {
        public static readonly string RemoveDupesSql = 
            @"with cte as (
	            select *, ROW_NUMBER() over (
		            PARTITION by title
		            order by id
		            ) row_num
		            from Movie
            )
            delete from cte
            where row_num > 1";
    }
}
