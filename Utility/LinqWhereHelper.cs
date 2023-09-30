using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Utility
{
    public class LinqWhereHelper<T>
        where T : class
    {
        public ParameterExpression param = Expression.Parameter(typeof(T), "c");
        public List<Expression> filters = new List<Expression>();
        public List<ExpressionFilter> listFilter = new List<ExpressionFilter>();
        public int filterIndex = 0;

        public LinqWhereHelper()
        {
            filters.Add(null);
        }

        public Expression<Func<T, bool>> GetExpression()
        {
            Expression expression = null;
            MakeNewFilter();
            foreach (ExpressionFilter tempFilter in listFilter)
            {
                if (expression == null)
                {
                    expression = tempFilter.Expression;
                    continue;
                }
                switch (tempFilter.Type)
                {
                    case "and":
                        expression = Expression.And(expression, tempFilter.Expression);
                        break;
                    case "or":
                        expression = Expression.Or(expression, tempFilter.Expression);
                        break;
                }
            }

            if (expression == null) return null;
            return Expression.Lambda<Func<T, bool>>(expression, param);
        }

        public void Equal(string propertyName, object value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Equal(left, right);
            InternelAddOr(result, isAnd);
        }

        public void NotEqual(string propertyName, object value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.NotEqual(left, right);
            InternelAddOr(result, isAnd);
        }

        public void MoreThan(string propertyName, object value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.GreaterThan(left, right);
            InternelAddOr(result, isAnd);
        }

        public void MoreThan(string propertyName, DateTime value, bool isAnd, bool isNull = true)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = GetDateTimeRight(value, isNull);
            Expression result = Expression.GreaterThan(left, right);
            InternelAddOr(result, isAnd);
        }

        public void MoreEqualThan(string propertyName, DateTime value, bool isAnd, bool isNull = true)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = GetDateTimeRight(value, isNull);
            Expression result = Expression.GreaterThanOrEqual(left, right);
            InternelAddOr(result, isAnd);
        }

        public void LessThan(string propertyName, object value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.LessThan(left, right);
            InternelAddOr(result, isAnd);
        }

        public void LessThan(string propertyName, DateTime value, bool isAnd, bool isNull = true)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = GetDateTimeRight(value, isNull);
            Expression result = Expression.LessThan(left, right);
            InternelAddOr(result, isAnd);
        }

        public void LessEqualThan(string propertyName, DateTime value, bool isAnd, bool isNull = true)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = GetDateTimeRight(value, isNull);
            Expression result = Expression.LessThanOrEqual(left, right);
            InternelAddOr(result, isAnd);
        }

        public void Contains(string propertyName, string value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
            InternelAddOr(result, isAnd);
        }

        public void Contains<X>(string propertyName, List<X> value, bool isAnd)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(right, typeof(List<X>).GetMethod("Contains"), left);
            InternelAddOr(result, isAnd);
        }

        public void InternelAddOr(Expression express, bool isAnd)
        {
            if (filters[filterIndex] == null)
            {
                filters[filterIndex] = express;
                return;
            }
            if (isAnd)
            {
                filters[filterIndex] = Expression.And(filters[filterIndex], express);
            }
            else
            {
                filters[filterIndex] = Expression.Or(filters[filterIndex], express);
            }
        }

        public void MakeNewFilter()
        {
            if (filters.Count == filterIndex + 1)
            {
                Expression tempExpression = filters[filterIndex];
                if (tempExpression != null)
                {
                    listFilter.Add(new ExpressionFilter()
                    {
                        Type = "and",
                        Expression = tempExpression
                    });
                }
            }
            filters.Add(null);
            filterIndex++;
        }

        public void MakeNewOrFilter()
        {
            if (filters.Count == filterIndex + 1)
            {
                Expression tempExpression = filters[filterIndex];
                if (tempExpression != null)
                {
                    listFilter.Add(new ExpressionFilter()
                    {
                        Type = "or",
                        Expression = tempExpression
                    });
                }
            }
            filters.Add(null);
            filterIndex++;
        }

        public LinqWhereHelper<T> CopyTo()
        {
            LinqWhereHelper<T> temp = new LinqWhereHelper<T>();
            temp.filterIndex = this.filterIndex;
            temp.param = this.param;
            Expression[] array = new Expression[this.filters.Count];
            this.filters.CopyTo(array);
            temp.filters = array.ToList();
            ExpressionFilter[] list = new ExpressionFilter[this.listFilter.Count];
            this.listFilter.CopyTo(list);
            temp.listFilter = list.ToList();

            return temp;
        }

        private Expression GetDateTimeRight(DateTime value, bool isNull)
        {
            Expression right = null;
            if (isNull)
            {
                right = Expression.Constant(value, typeof(DateTime?));
            }
            else
            {
                right = Expression.Constant(value, typeof(DateTime));
            }
            return right;
        }
    }

    public class ExpressionFilter
    {
        public ExpressionFilter()
        {

        }

        public string Type { get; set; }
        public Expression Expression { get; set; }
    }
}