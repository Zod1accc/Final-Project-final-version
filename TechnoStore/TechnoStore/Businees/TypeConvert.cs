using System.Reflection;

namespace TechnoStore.Businees
{
	public static class TypeConvert
	{
		public static TResult Convertion<T,TResult>(T model) where TResult : class,new()
		{
			TResult result = new TResult();

			typeof(T).GetProperties().ToList().ForEach(p =>
			{
				PropertyInfo property = typeof(TResult).GetProperty(p.Name);
				property?.SetValue(result, p.GetValue(model));

			});

			return result;
		}

		public static TResult Convertion<T,TResult>(T model,TResult exisObj)
		{
			typeof(T).GetProperties().ToList().ForEach(p =>
			{
				if(p.GetValue(model) is not null)
				{
					var property = typeof(TResult).GetProperty(p.Name);
					property?.SetValue(exisObj, p.GetValue(model));
				}
				
			});

			return exisObj;
		}
	}
}
