using OpenAI.Graders;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;


namespace ConsoleAppAI
{
    /*
    LINQ(Language-Integrated Query) :
    Is a set of features introduced in .NET Framework 3.5 (C# 3.0) that allows querying data sources(e.g., in-memory collections, databases, XML)
    using a SQL-like syntax integrated into C#.
    It provides a unified way to query data regardless of the source.

    KEY FEATURES:
    Type Safety: Queries are checked at compile-time, reducing runtime errors.
    Intuitive Syntax: Uses query expressions or method syntax for readable queries.
    Extensibility: Works with any data source implementing IEnumerable<T> or IQueryable<T>.
    Deferred Execution: Queries are executed only when results are needed, improving performance.
    Versatility: Supports filtering, sorting, grouping, joining, and aggregating data.

    Architecture of LINQ :
    LINQ’s architecture is modular, built on a set of components that enable querying various data sources.
    Components :
    1. LINQ Providers: Translate LINQ queries into specific data source operations.
        o LINQ to Objects: Queries in-memory collections (IEnumerable<T>).
        o LINQ to SQL: Queries relational databases (deprecated, replaced by Entity Framework).
        o LINQ to Entities: Queries Entity Framework data models (IQueryable<T>).
        o LINQ to XML: Queries XML documents.
        o LINQ to JSON: Queries JSON data (e.g., via System.Text.Json).
        o Custom Providers: Developers can create providers for other data sources.
    2. Standard Query Operators: Methods like Where, Select, OrderBy that operate on sequences.
    3. Query Expression Syntax: SQL-like syntax (e.g., from...where...select).
    4. Lambda Expressions: Underpin method syntax queries (e.g., numbers.Where(x => x > 0)).
    5. Expression Trees: Represent queries as data structures for database providers, enabling deferred
    execution.


    */
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int DepartmentId { get; set; }
        public List<int> Marks { get; set; }   // For SelectMany and aggregates
    }

    public class Department
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
    }

    public class LinqClass
    {
        //Correct entry point for C# Main
        static void Main()
        {
            List<Department> departments = new List<Department>
            {
                new Department { Id = 1, DeptName = "Computer Science" },
                new Department { Id = 2, DeptName = "Electronics" },
                new Department { Id = 3, DeptName = "Mechanical" }
            };

            List<Student> students = new List<Student>
            {
                new Student { Id = 1, Name = "Amit",    Age = 21, DepartmentId = 1, Marks = new List<int>{ 88, 76, 91 } },
                new Student { Id = 2, Name = "Sneha",   Age = 22, DepartmentId = 2, Marks = new List<int>{ 92, 85, 78 } },
                new Student { Id = 3, Name = "Rahul",   Age = 20, DepartmentId = 1, Marks = new List<int>{ 70, 65, 60 } },
                new Student { Id = 4, Name = "Priya",   Age = 23, DepartmentId = 3, Marks = new List<int>{ 89, 90, 95 } },
                new Student { Id = 5, Name = "Kiran",   Age = 21, DepartmentId = 2, Marks = new List<int>{ 55, 60, 58 } }
            };

            //OpenAIDemo.Run();   //THIS IS RUNNING OPENAI DEMO FILE

            Console.WriteLine("------LINQ--------");
            Console.WriteLine("-----Departments---");
            foreach(Department dep in departments)
            {
                Console.WriteLine(dep.Id);
                Console.WriteLine(dep.DeptName);
            }

            Console.WriteLine("-----Students-----");
            foreach(Student stud in students)
            {
                Console.WriteLine(stud.Id);
                Console.WriteLine(stud.Name);
                Console.WriteLine(stud.Marks[0]);
            }



            /*
             Different Ways to Write LINQ Queries in C#
             LINQ supports two syntaxes:
            1. Query Expression Syntax: SQL-like, readable for complex queries.
            2. Method Syntax: Uses extension methods and lambda expressions, more concise for simple queries.
            */
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine("Query Syntax:");
            var querySyntax = from num in numbers
                              where num > 2
                              select num;
            foreach (var num in querySyntax)
            {
                Console.WriteLine(num); // Output: 3, 4, 5
            }

            Console.WriteLine("Method Syntax:");
            var methodSyntax = numbers.Where(x => x > 2);           
            foreach (var num in methodSyntax)
            {
                Console.WriteLine(num); // Output: 3, 4, 5
            }
            //Both syntaxes produce the same result, but query syntax is more readable for complex queries,
            //while method syntax is concise for simple ones.



            /*
            IEnumerable and IQueryable in C#
            IEnumerable<t></ t >
                 Definition: Represents an in-memory collection that supports enumeration(e.g., List<T>, arrays).
                 Namespace: System.Collections.Generic.
                 Execution: Queries execute in-memory(LINQ to Objects).
                 Use Case: Querying local collections like lists or arrays.
            IQueryable<t> </ t >
                 Definition: Represents a queryable data source(e.g., database) that supports expression trees for
                deferred execution.
                 Namespace: System.Linq.
                 Execution: Queries are translated into provider - specific commands(e.g., SQL for databases).
                 Use Case: Querying remote data sources like databases via Entity Framework.
            */

            // IEnumerable Example
            List<int> nums = new List<int> { 1, 2, 3, 4, 5 };
            IEnumerable<int> evenNumbers = nums.Where(x => x % 2 == 0);
            Console.WriteLine("IEnumerable:");
            foreach (var num in evenNumbers)
            {
                Console.WriteLine(num); // Output: 2, 4
            }

            // IQueryable Example (simulated with AsQueryable)
            IQueryable<int> queryableNumbers = numbers.AsQueryable().Where(x => x % 2 == 0);
            Console.WriteLine("IQueryable:");
            foreach (var num in queryableNumbers)
            {
                Console.WriteLine(num); // Output: 2, 4
            }
            //Explanation: IEnumerable processes data in-memory, while IQueryable (simulated here) could translate to SQL in a database context.



            /*
            LINQ Extension Methods in C#:
            LINQ provides a set of extension methods in the System.Linq namespace, operating on IEnumerable<T> or
            IQueryable<T>.These methods are the backbone of LINQ queries, categorized by functionality(e.g., filtering,
            sorting, grouping).

            Categories of LINQ Operators
                1. Filtering: Where, OfType.
                2. Projection: Select, SelectMany.
                3. Set: Distinct, Union, Intersect, Except, Concat.
                4. Ordering: OrderBy, OrderByDescending, ThenBy, ThenByDescending, Reverse.
                5. Aggregate: Sum, Min, Max, Average, Count, Aggregate.
                6. Quantifiers: All, Any, Contains.
                7. Element: First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault, ElementAt,
                ElementAtOrDefault, DefaultIfEmpty.
                8. Partitioning: Take, TakeWhile, Skip, SkipWhile.
                9. Generation: Range, Repeat, Empty.
                10. Conversion: ToList, ToArray, ToDictionary, Cast, OfType.
                11. Joining: Join, GroupJoin.
                12. Grouping: GroupBy, ToLookup.
                13. Sequence: SequenceEqual, Zip.
            */

            #region LINQ SELECT, SELECTMANY, WHERE, OFTYPE
            Console.WriteLine("-----SELECT-------");
            //SELECT OPERATOR:
            //Select transforms each element
            //It applies a projection 1 input → 1 output.
            //Output is always same number of elements as input (unless filtered).
            var stmt = departments.Select(x => x.DeptName);
            foreach(var d in stmt)     //you cannot use indexing ([0], [1]) on an IEnumerator, that's why using foreach
            {
                Console.WriteLine(d);
            }

            //If you need indexing for selstmt we need to convert to ToList();
            var stmt1 = departments.Select(x => x.DeptName).ToList();
            Console.WriteLine(stmt1[0]);

            var stmt2 = departments.Select(x => x.DeptName).ToList().First(); //gives only first element 
            Console.WriteLine(stmt2);
           
            var res = students.Select(s => s.Marks);
            //Output:   //[
            //   [88, 76, 91],
            //   [92, 85, 78]
            //]
            //Select keeps the nested list structure.


            Console.WriteLine("-----SELECTMANY-------");
            //SELECTMANY OPERATOR:
            //SelectMany flattens the result
            //It applies a projection 1 input → many outputs, and then flattens them into a single sequence.
            //Output count increases
            //(total marks of all students).
            var res1 = students.SelectMany(s => s.Marks);
            foreach(var s in res1)
            {
                Console.WriteLine(s);
            }
            //output:  88 76,91,92,85,78,70,65,60,89,90,95,55,60,58  
            //SelectMany removes nesting
            //Turns List<List< int >> into List<int>

            var details = students.SelectMany(
                s => s.Marks,
                (student, mark) => new { student.Name, mark }
            );
            foreach (var s in details)
            {
                Console.WriteLine(s);
            }
            //output:
            //{ Name = Amit, mark = 88 }
            //{ Name = Amit, mark = 76 }
            //{ Name = Amit, mark = 91 }
            //{ Name = Sneha, mark = 92 }
            //{ Name = Sneha, mark = 85 } etc....




            Console.WriteLine("-----WHERE-------");
            //WHERE OPERATOR:
            //Filters a sequence based on a predicate
            var res2 = students.Where(x => x.Id % 2 == 0);
            foreach(var s in res2)
            {
                Console.WriteLine(s.Id);
                Console.WriteLine(s.Name);
            }



            Console.WriteLine("-----ofType-------");
            //OFTYPE OPERATOR:
            //Filters elements of a specific type from a sequence.
            List<object> values = new()
            {
                1,
                "Hello",
                50,
                new Department { Id = 10, DeptName = "CS" }
            };

            var ints = values.OfType<int>();
            foreach(var v in ints)
                Console.WriteLine(v);
            #endregion  




            #region SET OPERATORS
            Console.WriteLine("-------SET OPERATORS-------");
            //LINQ Distinct Method
            //Removes duplicate elements from a sequence.
            List<int> numList = new List<int> { 1, 2, 2, 3, 3, 4 };
            var distinct = numList.Distinct();
            Console.WriteLine(string.Join(", ", distinct));        //----> Instead of foreach I am using string.Join() from now on
            // Output: 1, 2, 3, 4

            //LINQ Except Method
            //Returns elements in the first sequence not in the second.
            List<int> set1 = new List<int> { 1, 2, 3, 4 };
            List<int> set2 = new List<int> { 3, 4, 5 };
            var except = set1.Except(set2);
            Console.WriteLine(string.Join(", ", except)); // Output: 1, 2

            //LINQ Intersect Method
            //Returns elements common to both sequences.
            List<int> set3 = new List<int> { 1, 2, 3, 4 };
            List<int> set4 = new List<int> { 3, 4, 5 };
            var intersect = set3.Intersect(set4);
            Console.WriteLine(string.Join(", ", intersect)); // Output: 3, 4

            //LINQ Union Method
            //Returns unique elements from both sequences. removing duplicates.
            List<int> set5 = new List<int> { 1, 2, 3 };
            List<int> set6 = new List<int> { 3, 4, 5 };
            var union = set5.Union(set6);
            Console.WriteLine(string.Join(", ", union)); // Output: 1, 2, 3, 4, 5

            //LINQ Concat Method
            //Concatenates two sequences without removing duplicates.
            List<int> set7 = new List<int> { 1, 2, 3 };
            List<int> set8 = new List<int> { 3, 4, 5 };
            var concat = set7.Concat(set8);
            Console.WriteLine(string.Join(", ", concat)); // Output: 1, 2, 3, 3, 4, 5
            #endregion




            #region Ordering Operators
            Console.WriteLine("--------Ordering Operators-------");

            Console.WriteLine("------OrderBy------");
            //OrderBy is used to sort a collection in ascending order based on a key (a property or expression).
            //It returns a new sorted sequence — it does not modify the original list.
            var numss = new List<int> { 5, 2, 9, 1, 6 };
            var sorted = numss.OrderBy(n => n);
            Console.WriteLine(string.Join('+', sorted));

            var sortedStudents = students.OrderBy(s => s.Age);
            Console.WriteLine(string.Join('-', sortedStudents.Select(s => s.Age)));  //sortedStudents is a list of Student objects, and string.Join needs strings.

            //✔ It does NOT modify the original list
            //The original students list stays unchanged.
            //✔ Sorting is deferred
            //If the list changes later, the sorted sequence changes too.
            //Unless you force execution using .ToList():

            var sortedStudents1 = students.OrderBy(s => s.Marks.Average());  //Sorting by a property inside a nested object
            Console.WriteLine(string.Join('-', sortedStudents1.Select(x => x.Name))); //whose average is low prints first


            Console.WriteLine("-------OrderByDescending-------");
            //Sorts a sequence in descending order.
            List<int> nums1 = new List<int> { 1, 3, 2, 5, 4 };
            var sortedDesc = nums1.OrderByDescending(n => n);
            Console.WriteLine(string.Join(", ", sortedDesc)); // Output: 5, 4, 3, 2, 1

            var sortedStudentsDesc = students.OrderByDescending(s => s.Marks.Average());
            Console.WriteLine(string.Join('-', sortedStudentsDesc.Select(x => x.Name))); //whose average is high to low 


            Console.WriteLine("-------ThenBy and ThenByDescending-----------");
            //Performs secondary sorting after OrderBy or OrderByDescending.
            var sorted1 = students
                .OrderBy(s => s.DepartmentId)   // primary
                .ThenBy(s => s.Age);            // secondary
            foreach(var s in sorted1)
                Console.WriteLine(s.Name+' '+' '+s.DepartmentId+' '+s.Age);

            var sorted2 = students
                .OrderBy(s => s.DepartmentId)        // primary ASC
                .ThenByDescending(s => s.Age);       // secondary DESC
            foreach (var s in sorted2)
                Console.WriteLine(s.Name + ' ' + ' ' + s.DepartmentId + ' ' + s.Age);


            Console.WriteLine("------Reverse-------");
            //Reverse() reverses the order of the items in a sequence.
            //📌 Important:
            //It does NOT sort
            //It simply flips the order
            //First becomes last, last becomes first
            //It works on any IEnumerable<T>
            ///Linq Reverse() does NOT modify the original list
           
            //Reverse using List<T> --> modifies the original list
            var numbers1 = new List<int> { 10, 20, 30, 40 };
            numbers1.Reverse();                                //--> returns void
            Console.WriteLine(string.Join(", ", numbers1));

            var numbers2 = new List<int> { 10, 20, 30, 40 };
            var reversed = numbers2.AsEnumerable().Reverse();      //--> returns IEnumerable<T>
            Console.WriteLine(string.Join(", ", reversed));
            #endregion




            #region Aggragate Operators
            Console.WriteLine("--------LINQ Aggregate Operators-----------");

            Console.WriteLine("----Sum-----");
            //Sum() is an aggregate operator that:
            //✔ Adds all numeric values in a collection
            //✔ Returns the total
            //✔ Works on any numeric collection(int, float, double, decimal, long)
            //✔ Works with selectors(e.g., Sum(x => x.Property))
            List<int> numbers3 = new List<int> { 1, 2, 3, 4, 5 };
            var sum = numbers3.Sum();
            Console.WriteLine($"Sum: {sum}"); // Output: Sum: 15

            var sumAges = students.Sum(s => s.Age);
            Console.WriteLine(sumAges);

            var totalMarks = students.Sum(s => s.Marks.Sum());
            Console.WriteLine(totalMarks);

            //with where condition
            var total = students
                .Where(s => s.Age > 21)
                .Sum(s => s.Marks.Sum());
            Console.WriteLine(total);

            //with SelectMany
            var total1 = students
                .SelectMany(s => s.Marks)
                .Sum();
            Console.WriteLine(total);

            //var totalSalary = employees.Sum(e => e.Salary);


            Console.WriteLine("----Max-----");
            //✔ Returns The maximum value in a sequence
            //✔ Highest number
            //✔ Highest property value(with a selector)
            List<int> numbers4 = new List<int> { 1, 5, 3, 4, 2 };
            var max = numbers4.Max();
            Console.WriteLine($"Max: {max}"); // Output: Max: 5

            var maxAge = students.Max(s => s.Age);
            Console.WriteLine(maxAge);

            var maxAvgMarks = students.Max(s => s.Marks.Average());
            Console.WriteLine(maxAvgMarks);

            var highestMark = students.SelectMany(s => s.Marks).Max();
            Console.WriteLine(highestMark);

            var names = new List<string> { "Amit", "Sneha", "Rahul" };
            var maxName = names.Max();
            Console.WriteLine(maxName);


            Console.WriteLine("----Min-----");
            //✔ Returns The minimum value in a sequence
            var numbers5 = new List<int> { 10, 20, 30, 40 };
            var min = numbers5.Min();
            Console.WriteLine(min);

            var minAge = students.Min(s => s.Age);
            Console.WriteLine(minAge);

            var minMark = students.SelectMany(s => s.Marks).Min();
            Console.WriteLine(minMark);

            var minAvg = students.Min(s => s.Marks.Average());
            Console.WriteLine(minAvg);


            Console.WriteLine("-----Average------");
            //Average() returns the mean (sum / count) of a numeric sequence.
            var numbers6 = new List<int> { 10, 20, 30, 40 };
            var avg = numbers5.Average();
            Console.WriteLine(avg);

            //Average age of students
            var avgAge = students.Average(s => s.Age);
            //Average of all student marks
            var avgAllMarks = students.SelectMany(s => s.Marks).Average();
            //Average marks per student
            var avgMarks = students.Select(s => new {
                s.Name,
                Avg = s.Marks.Average()
            });

            Console.WriteLine("-----Count------");
            //Returns how many elements are in a sequence.
            var totalStudents = students.Count();                 //Count all students
            var countOlder = students.Count(s => s.Age > 21);     //Count with a condition (predicate)
            var totalMarksCount = students.SelectMany(s => s.Marks).Count();   //Count total marks of all students
            var csCount = students.Count(s => s.DepartmentId == 1);     //Count department 1 students


            Console.WriteLine("----Aggregate------");
            //Aggregate() allows you to perform custom accumulation operations over a sequence.
            var numbers7 = new List<int> { 10, 20, 30 };
            var sum1 = numbers7.Aggregate((a, b) => a + b);    //same as .sum
            Console.WriteLine(sum1);

            var result = numbers.Aggregate((a, b) => a * b);   //Multiply all numbers

            var totalMarks1 = students               //Total marks of ALL students
                .SelectMany(s => s.Marks)
                .Aggregate((a, b) => a + b);

            var result1 = students.Select(s => new {    //Total marks of each student using Aggregate
                s.Name,
                Total = s.Marks.Aggregate((a, b) => a + b)
            });

            //Join strings
            var words = new List<string> { "Amit", "Sneha", "Rahul" };
            var sentence = words.Aggregate((a, b) => a + ", " + b);
            #endregion




            #region Quantifier Operators
            Console.WriteLine("---------LINQ Quantifiers Operators-------");
            Console.WriteLine("----All----");
            //Checks if all elements satisfy a condition.
            List<int> numbers8 = new List<int> { 2, 4, 6, 8 };
            bool allEven = numbers8.All(n => n % 2 == 0);
            Console.WriteLine($"All even: {allEven}"); // Output: All even: True

            Console.WriteLine("----Any----");
            //Checks if any element satisfies a condition.
            List<int> numbers9 = new List<int> { 1, 3, 5, 6 };
            bool hasEven = numbers9.Any(n => n % 2 == 0);
            Console.WriteLine($"Has even: {hasEven}"); // Output: Has even: True

            Console.WriteLine("---Contains----");
            //Checks if a sequence contains a specific element.
            List<string> names1 = new List<string> { "Alice", "Bob", "Charlie" };
            bool containsBob = names.Contains("Bob");
            Console.WriteLine($"Contains Bob: {containsBob}"); // Output: Contains Bob: True
            #endregion







            //DEFERRED EXECUTION vs IMMEDIATE EXECUTION in LINQ
            //Deferred Execution:
                // Queries are not executed until the results are enumerated(e.g., via foreach, ToList).
                // Allows building complex queries incrementally without immediate performance cost.
                // Example Operators: Where, Select, OrderBy.
            //Immediate Execution:
                // Queries execute immediately, producing results.
                // Example Operators: Count, Sum, ToList, ToArray.
            List<int> numbersA = new List<int> { 1, 2, 3 };
            var query = numbersA.Where(n => { Console.WriteLine($"Checking {n}"); return n > 1; }); // Deferred
            Console.WriteLine("Query defined");
            foreach (var num in query) // Execution happens here
            {
                Console.WriteLine(num);
            }
            var count = numbersA.Count(n => { Console.WriteLine($"Counting {n}"); return n > 1; }); // Immediate
            Console.WriteLine($"Count: {count}");





            #region GroupBy, ToLookup
            Console.WriteLine("--------GroupBy--------");
            //✔ Groups are lazy(deferred execution)
            //✔ A group may contain 1 or many items
            //✔ Key is accessed via group.Key
            //✔ You must iterate a group like a list

            //Groups elements based on a key.
            List<string> names2 = new List<string> { "Alice", "Bob", "Charlie", "Ann" };
            var grouped = names2.GroupBy(n => n[0]);
            foreach (var group in grouped)
            {
                Console.WriteLine($"Group: {group.Key}");
                foreach (var name in group)
                {
                    Console.WriteLine($" {name}");
                }
            }
            // Output:
            // Group: A
            // Alice
            // Ann
            // Group: B
            // Bob
            // Group: C
            // Charlie


            //Group students by DepartmentId
            var groups = students.GroupBy(s => s.DepartmentId);
            foreach (var group in groups)
            {
                Console.WriteLine($"Department {group.Key}:");

                foreach (var s in group)
                    Console.WriteLine($"  {s.Name}");
            }
            //Department 1:
            //  Amit
            //  Rahul
            //Department 2:
            //  Sneha
            //  Kiran
            //Department 3:
            //  Priya


            //Group students by Age
            var groups1 = students.GroupBy(s => s.Age);

            //Group by Pass / Fail(custom key)
            var groups2 = students.GroupBy(s => s.Marks.Average() >= 60 ? "Pass" : "Fail");
            //Pass:
            //   Amit, Sneha, Rahul, Priya
            //Fail:
            //    Kiran

            //Group + Projection (Select after GroupBy)
            var result2 = students
                .GroupBy(s => s.DepartmentId)
                .Select(g => new {
                    Department = g.Key,
                    Count = g.Count()
                });

            //Group using multiple keys (anonymous type)
            var groups3 = students.GroupBy(s => new { s.DepartmentId, s.Age });
        



            Console.WriteLine("-------ToLookup------");
            //Creates a one - to - many dictionary(ILookup<TKey, TElement>) similar to GroupBy, but executes immediately.
            //ToLookup creates a lookup table(like a dictionary) where:
            //     Key = group key
            //     Value = collection(group) of items with that key
            //It behaves like a Dictionary<TKey, List< TValue >>
            //…but better because:

            //✔ It supports duplicate keys
            //✔ It is immutable
            //✔ It is always fully evaluated immediately
            //✔ It allows lookup[key] even if key doesn’t exist(returns empty)

            //syntax:
            //var lookup = collection.ToLookup(item => item.Key);
            //var lookup = collection.ToLookup(item => item.Key, item => item.Value);

            //Group students by DepartmentId using ToLookup
            var lookup = students.ToLookup(s => s.DepartmentId);
            foreach (var stud in lookup[1])
            {
                Console.WriteLine(stud.Name);  // Dept 1 students
            }
            //output:Amit Rahul

            //Access groups like a dictionary
            var csStudents = lookup[1];   // OK
            var mechStudents = lookup[3]; // OK
            var unknown = lookup[99];     // ✔ returns empty list (not error)


            //Difference from GroupBy:
            //| Feature       | GroupBy                 | ToLookup |
            //| ------------  | ----------------------  | ----------------------------- |
            //| Execution     | Deferred(lazy)          | Immediate(eager)              | 
            //| Index access  | ❌ No                   | ✔ Yes                        |
            //| Returns       | IEnumerable<IGrouping>  | Lookup<TKey, TValue>          |
            //| Missing key   | Throws / checks needed  | Returns empty sequence        |
            //| Purpose       | Querying                | Fast lookup(like dictionary)  |
            #endregion





            #region JOINS
            Console.WriteLine("-------LINQ Joins---------");         //majorly written on QUERY SYNTAX

            Console.WriteLine("----Inner Join-----");
            //An Inner Join returns only the matching records from both collections based on a common key.
            //QUERY SYNTAX
            var result3 = from stud in students
                         join department in departments
                         on stud.DepartmentId equals department.Id
                         select new
                         {
                             StudentName = stud.Name,
                             Age = stud.Age,
                             DepartmentName = department.DeptName
                         };

            foreach (var item in result3)
            {
                Console.WriteLine($"{item.StudentName} - {item.Age} - {item.DepartmentName}");
            }

            //METHOD SYNTAX
            var result4 = students.Join(
                departments,
                student => student.DepartmentId,   // outer key selector
                dept => dept.Id,                  // inner key selector
                (student, dept) => new            // result selector
                {
                    StudentName = student.Name,
                    Age = student.Age,
                    DepartmentName = dept.DeptName
                });

            foreach (var item in result4)
            {
                Console.WriteLine($"{item.StudentName} - {item.Age} - {item.DepartmentName}");
            }


            Console.WriteLine("------Group Join-------");
            //A Group Join creates a collection of grouped results.
            //It groups matching elements from one sequence (students)with another(departments).

            //✔ Unlike Inner Join:
            //Inner Join → returns flat matched rows
            //Group Join → returns each department + list of all students in that department

            //Query syntax
            var result5 = from dept in departments
                         join stud in students
                         on dept.Id equals stud.DepartmentId
                         into studentGroup   // grouped results
                         select new
                         {
                             DepartmentName = dept.DeptName,
                             Students = studentGroup
                         };

            foreach (var item in result5)
            {
                Console.WriteLine($"Department: {item.DepartmentName}");

                foreach (var s in item.Students)
                {
                    Console.WriteLine($"   {s.Name} ({s.Age})");
                }
            }

            //Method syntax
            var result6 = departments.GroupJoin(
                students,
                dept => dept.Id,              // outer key selector (Department)
                student => student.DepartmentId, // inner key selector (Student)
                (dept, studentGroup) => new
                {
                    DepartmentName = dept.DeptName,
                    Students = studentGroup
                });

            foreach (var item in result6)
            {
                Console.WriteLine($"Department: {item.DepartmentName}");

                foreach (var s in item.Students)
                {
                    Console.WriteLine($"   {s.Name} ({s.Age})");
                }
            }


            Console.WriteLine("-----LEFT JOIN-----");
            //A Left Join returns:
                //All records from the left collection(Departments)
                //Matched records from the right collection(Students)
                //If no match exists → returns null for the right side records
            //👉 In LINQ, Left Join is done by GroupJoin + DefaultIfEmpty()  //(because LINQ does not have a direct left join keyword)

            //Let's assume some departments have no students.
            //We will add one such department:
            departments.Add(new Department { Id = 4, DeptName = "Civil" });
            var result7 = from dept in departments
                         join stud in students
                         on dept.Id equals stud.DepartmentId
                         into studentGroup
                         from s in studentGroup.DefaultIfEmpty()     //If the group is empty → returns a single null value
                         select new
                         {
                             Department = dept.DeptName,
                             StudentName = s?.Name ?? "No Student",   //Null-safe operator → prevents NullReferenceException.
                             Age = s?.Age
                         };

            foreach (var item in result7)
            {
                Console.WriteLine($"{item.Department} - {item.StudentName}");
            }
            //Computer Science - Amit
            //Computer Science -Rahul
            //Electronics - Sneha
            //Electronics - Kiran
            //Mechanical - Priya
            //Civil - No Student


            Console.WriteLine("----RIGHT JOIN----");
            //A Right Join returns:
                //All records from the RIGHT collection
                //Matching records from the LEFT collection
                //If no match → returns null for the left-side values
            //In your example:
                //Right side → students
                //Left side → departments
            //So a Right Join must return all students, even if some students do not belong to a valid department.

            //Add an unmatched student to demonstrate Right Join
            //Deepak's DepartmentId = 10 → No department with Id 10 exists.
            students.Add(new Student { Id = 7, Name = "Abhi", Age = 22, DepartmentId = 10 });

            var rightJoin1 = from stud in students
                             join dept in departments
                             on stud.DepartmentId equals dept.Id
                             into deptGroup
                             from d in deptGroup.DefaultIfEmpty()
                             select new
                             {
                                 StudentName = stud.Name,
                                 DepartmentName = d?.DeptName ?? "No Department"
                             };

            foreach (var item in rightJoin1)
            {
                Console.WriteLine($"{item.StudentName} - {item.DepartmentName}");
            }


            Console.WriteLine("------FULL OUTER JOIN------");
            //A Full Outer Join returns:
                //All records from the left collection(Departments)
                //All records from the right collection(Students)
                //Matches when keys are equal
                //If no match exists → returns null on the missing side
            //👉 Unlike SQL, LINQ does NOT have a built -in Full Outer Join, so we must combine:
                //Left Join
                //Right Join
                //Union

            //Add a department with no students:
            departments.Add(new Department { Id = 4, DeptName = "Civil" });
            //Add a student who does not belong to any department:
            students.Add(new Student { Id = 6, Name = "Deepak", Age = 22, DepartmentId = 10 });

            var leftJoin = from d in departments
                           join s in students
                           on d.Id equals s.DepartmentId
                           into studentGroup
                           from s in studentGroup.DefaultIfEmpty()
                           select new
                           {
                               Department = d.DeptName,
                               Student = s?.Name
                           };

            var rightJoin = from s in students
                            join d in departments
                            on s.DepartmentId equals d.Id
                            into deptGroup
                            from d in deptGroup.DefaultIfEmpty()
                            select new
                            {
                                Department = d?.DeptName,
                                Student = s.Name
                            };

            var fullOuterJoin = leftJoin
                                .Union(rightJoin)
                                .Distinct();
            //Computer Science -Amit
            //Computer Science -Rahul
            //Electronics - Sneha
            //Electronics - Kiran
            //Mechanical - Priya
            //Civil - (No Student)
            //                  -Deepak

            //Explanation:
            //Civil → has no students → still appears
            //Deepak → belongs to unknown department → still appears


            //NOTE:
            //| Join Type           | Returns                            |
            //| ------------------- | ---------------------------------- |
            //| **Inner Join * *    | Only matched records               |
            //| **Left Join * *     | All left + matches + null on right |
            //| **Right Join * *    | All right + matches + null on left |
            //| **Full Outer Join** | All left + all right               |
            #endregion





            #region Element Operators
            Console.WriteLine("-----------Element Operators in LINQ------------");
            Console.WriteLine("------ElementAt and ElementAtOrDefault-------");
            //ElementAt(Index)
                //Returns the element at the specified index in a sequence.
            //❌ Throws exception if:
                //Index is out of range
            //Example: accessing index 10 in a list of 3 items

            var numbersB = new List<int> { 10, 20, 30, 40 };
            int value = numbersB.ElementAt(2);  // index 2 → 30
            Console.WriteLine(value);

            //If you try numbers.ElementAt(10);
            //System.ArgumentOutOfRangeException

            var secondStudent = students.ElementAt(1);
            Console.WriteLine(secondStudent.Name);
            //var invalid = students.ElementAt(10);    //System.ArgumentOutOfRangeException


            //ElementAtOrDefault(index)
                //Returns the element at the given index, or the default value if the index is out of range.
            //Default values:
                //For int → 0
                //For string → null
                //For custom class → null
            //✔ Does NOT throw exception
            var numbersC = new List<int> { 10, 20, 30, 40 };

            int value1 = numbersC.ElementAtOrDefault(2);   // 30
            int value2 = numbersC.ElementAtOrDefault(10);  // out of range → default int = 0

            Console.WriteLine(value1);
            Console.WriteLine(value2);

            var studentAt2 = students.ElementAtOrDefault(2);
            Console.WriteLine(studentAt2.Name);


            Console.WriteLine("--------First and FirstOrDefault-------");
            //First:
            //Returns the first element of a sequence.
            //❌ Throws an exception if:
                //The sequence is empty
                //No element matches the condition(when using a predicate)

            //Get the first student
            var firstStudent = students.First();
            Console.WriteLine(firstStudent.Name);

            //First student from Computer Science
            var csStudent = students.First(s => s.DepartmentId == 1);
            Console.WriteLine(csStudent.Name);

            //If no match found → Exception
            //var unknown = students.First(s => s.DepartmentId == 999);  //InvalidOperationException: Sequence contains no matching element


            //FirstOrDefault():
                //Returns the first element
            //✔ Returns default value if:
                //Sequence is empty
                //No matching element
            //Default value:
                //For classes → null
                //For numbers → 0
            //✔ Does NOT throw exception

            //Safe version of first student
            var firstStd = students.FirstOrDefault();
            Console.WriteLine(firstStd?.Name);

            //First student from Mechanical
            var mechStudent = students.FirstOrDefault(s => s.DepartmentId == 3);
            Console.WriteLine(mechStudent?.Name);

            //Example 3: When no match → returns null
            var noStudent = students.FirstOrDefault(s => s.DepartmentId == 999);
            if (noStudent == null)
                Console.WriteLine("No student found");




            Console.WriteLine("---------Single and SingleOrDefault---------");
            //Single:
                //✔ Returns the only element in a sequence.
            //❗ Throws an exception if:
                //The sequence has zero elements
                //The sequence has more than one matching element
            //👉 Use this only when you are SURE the result must be exactly one item.

            //Get the student with Id = 3
            var student = students.Single(s => s.Id == 3);
            Console.WriteLine(student.Name);               //Because Id 3 exists only once → works fine.

            ///More than one matching student
            //var csStudent1 = students.Single(s => s.DepartmentId == 1);
            //DepartmentId = 1 has two students: Amit  Rahul
            //InvalidOperationException: Sequence contains more than one matching element

            //No matching element
            //var noStd = students.Single(s => s.Id == 999);
            //InvalidOperationException: Sequence contains no matching element



            //SingleOrDefault():
                //✔ Returns the only element, if exactly one exists
            //✔ Returns default(null for classes) if:
                //No matching element
            //❗ But still throws exception if:
                //More than one matching element

            var priya = students.SingleOrDefault(s => s.Id == 4);
            Console.WriteLine(priya?.Name);

            //No match → returns default (null)
            var unknown1 = students.SingleOrDefault(s => s.Id == 999);
            if (unknown1 == null)
                Console.WriteLine("No student found");

            //More than one match → exception
            //var dept1Student = students.SingleOrDefault(s => s.DepartmentId == 1);  //InvalidOperationException: Sequence contains more than one matching element




            Console.WriteLine("----DefaultIfEmpty----");
            //DefaultIfEmpty() returns:
                //The original sequence, if it has elements
                //A sequence with one default value, if the original sequence is empty
            //Default values:
                //For class → null
                //For int → 0
                //For string → null
            //👉 It is mainly used inside Left Join.

            //Normal List (Not Empty)
            var studentList = students.DefaultIfEmpty();
            foreach (var s in studentList)
            {
                Console.WriteLine(s.Name);
            }

            //An Empty List
            var emptyList = new List<Student>();
            var resultA = emptyList.DefaultIfEmpty();
            foreach (var s in resultA)
            {
                Console.WriteLine(s == null ? "Default (null)" : s.Name);     //Default (null)
            }



            Console.WriteLine("--------SequenceEqual----------");
            //SequenceEqual();
            //Compares two sequences element - by - element and returns true if:
                //Both sequences have the same number of elements
                //Elements are in the same order
                //Elements are equal
            //Order matters
            //{ 10,20,30} ≠ { 30,20,10}

            //Compare Two Lists of Integers
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 1, 2, 3 };

            bool areEqual = list1.SequenceEqual(list2);
            Console.WriteLine(areEqual);                    //True

            //Order matters
            var list3 = new List<int> { 3, 2, 1 };
            bool resultB = list1.SequenceEqual(list3);
            Console.WriteLine(resultB);                 //false

            //using students list
            var studentsListA = students;
            var studentsListB = students.ToList();   // exact copy
            bool same = studentsListA.SequenceEqual(studentsListB);
            Console.WriteLine(same);            //True

            //| Operator                        | Returns                              | Throws Exception ?           | When to Use                                  |
            //| ----------------------------- | ------------------------------------   | -----------------------------| -------------------------------------------- |
            //| **First() * *                   | First element                        | Yes(if empty or no match)    | When sequence MUST have at least one element |
            //| **FirstOrDefault() * *          | First element, or default if none    | No                           | When sequence may be empty                   |
            //| **Last() * *                    | Last element                         | Yes                          | When at least one element exists             |
            //| **LastOrDefault() * *           | Last element, or default             | No                           | When list may be empty                       |
            //| **Single() * *                  | The ONLY element                     | Yes(if zero or more than one)| When exactly one item must exist             |
            //| **SingleOrDefault() * *         | The only element, or default if none | Yes(if more than one)        | When zero or one match is expected           |
            //| **ElementAt(index) * *          | Element at given index               | Yes(if index invalid)        | When you know index exists                   |
            //| **ElementAtOrDefault(index) * * | Element at index or default          | No                           | Safe index access                            |
            //| **DefaultIfEmpty() * *          | Same sequence, or 1 default element if empty | No                   | Left Join / Avoid empty sequence             |
            //| **SequenceEqual(seq2) * *       | True if both sequences have same elements in same order | No        | Compare lists for equality                   |
            #endregion






            #region Partitioning Operators
            Console.WriteLine("------LINQ Partitioning Operators-------");
            Console.WriteLine("---Take()-----");
            //Take(n) returns the first n elements from a sequence.
            //✔ If the sequence has more than n items → returns first n items
            //✔ If the sequence has fewer than n items → returns all items
            //✔ Does NOT modify the original list
            //✔ Does NOT throw exceptions(safe)

            //Take first 3 students
            var firstThree = students.Take(3);
            foreach (var s in firstThree)
            {
                Console.WriteLine(s.Name);
            }

            //Take first 10 (more than total count)
            var resultC = students.Take(10);
            foreach (var s in resultC)
            {
                Console.WriteLine(s.Name);           //It simply returns all students, no error.
            }

            //Take top 2 highest age
            var oldest = students.OrderByDescending(s => s.Age).Take(2);
            foreach (var s in oldest)
            {
                Console.WriteLine($"{s.Name} - {s.Age}");
            }



            Console.WriteLine("-----TakeWhile()-----");
            //TakeWhile() returns elements from the beginning of a sequence as long as the condition is true.
            //✔ It stops as soon as the condition becomes false
            //✔ After it stops → it does not check or return any later elements, even if they match
            //👉 It is a conditional version of Take().

            //Take students while age > 20
            var resultD = students.TakeWhile(s => s.Age > 20);
            foreach (var s in resultD)
            {
                Console.WriteLine($"{s.Name} - {s.Age}");
            }

            //Using index in TakeWhile
            var resultE = students.TakeWhile((s, index) => index < 3);
            foreach (var s in resultE)
            {
                Console.WriteLine(s.Name);
            }



            Console.WriteLine("-------Skip(count)--------");
            //Skip(count)
            //Skip(n) skips the first n elements and returns the rest.
            //✔ If n > total count → returns empty sequence
            //✔ Never throws exception(safe)

            //Skip first 2 students
            var resultF = students.Skip(2);
            foreach (var s in resultF)
            {
                Console.WriteLine(s.Name);
            }


            Console.WriteLine("---------SkipWhile(predicate)--------");
            //SkipWhile() skips elements as long as the condition is true.
            //The moment the condition becomes false, it STOPS skipping
            //→ and returns ALL remaining elements, even if they match again.
            //👉 Opposite of TakeWhile()

            //Skip while age > 20
            var resultG = students.SkipWhile(s => s.Age > 20);
            foreach (var s in resultG)
            {
                Console.WriteLine($"{s.Name} - {s.Age}");
            }

            //Important: SkipWhile checks from the beginning only
            //Once the condition becomes false, it does not check again.


            //SUMMARY
            //| Operator                   | What It Does                                          | Stops When ?             | Behavior          | Example Output(Students List)         |
            //| ------------------------   | --------------------------------------------------    | -----------------------  | ------------------| ------------------------------------- |
            //| **Take(n) * *              | Takes the** first n elements**                        | After taking n items     | Fixed count       | `Take(2)` → Amit, Sneha               |
            //| **TakeWhile(condition) * * | Takes items** from start while condition is true * *  | Condition becomes false  | Condition - based | `TakeWhile(s.Age > 20)` → Amit, Sneha |
            //| **Skip(n) * *              | Skips the** first n elements**                        | After skipping n items   | Fixed count       | `Skip(2)` → Rahul, Priya, Kiran       |
            //| **SkipWhile(condition) * * | Skips items** from start while condition is true * *  | Condition becomes false  | Condition - based | `SkipWhile(s.Age > 20)` → Rahul, Priya, Kiran |
            #endregion




            #region Generation Operators

            Console.WriteLine("--------LINQ Generation Operators---------");
            Console.WriteLine("-----Enumerable.Range()-----");
            //Generates a sequence of consecutive integers.
            var numbersD = Enumerable.Range(1, 5);             //--> this creates Ienumerable
            Console.WriteLine(string.Join('-',numbersD));
            //usecase: Create loops, dummy data, or test sequences.

            Console.WriteLine("-------Enumerable.Repeat()-----");
            //Creates a sequence where the same value is repeated.
            var repeated = Enumerable.Repeat("Hello", 3);
            Console.WriteLine(string.Join('-', repeated));
            //usecase: Initialize arrays, placeholders, test values.

            Console.WriteLine("-------Enumerable.Empty<T>()------");
            //Returns an empty sequence of type T, without allocating new memory.
            var emptyStudents = Enumerable.Empty<Student>();
            Console.WriteLine("hi" + string.Join("-",emptyStudents));
            //usecase: Avoid returning null → return empty sequence instead.

            Console.WriteLine("------Append()------");
            ///Adds one element to the END of a sequence
            var updatedList = students.Append(new Student
            {
                Id = 10,
                Name = "Suresh",
                Age = 22,
                DepartmentId = 10
            });
            //Note: Append does not modify original list, it returns a new sequence.

            Console.WriteLine("------Prepend()-------");
            //Adds one element to the START of a sequence.
            var updatedList1 = students.Prepend(new Student
            {
                Id = 11,
                Name = "Admin",
                Age = 30,
                DepartmentId = 11
            });

            Console.WriteLine("------Zip()-------");
            //Combines two sequences element-by-element.
            //The result contains pairs
            //Stops when the shortest sequence ends

            //Zipping two integer lists
            var numbersI = new[] { 1, 2, 3 };
            var wordsI = new[] { "One", "Two", "Three" };
            var resultI = numbersI.Zip(wordsI, (n, w) => $"{n} - {w}");
            //1 - One
            //2 - Two
            //3 - Three

            //Zip with your students (Name + Age)
            var names3 = students.Select(s => s.Name);
            var ages = students.Select(s => s.Age);
            var combined = names3.Zip(ages, (n, a) => $"{n} ({a})");
            foreach (var item in combined)
                Console.WriteLine(item);


            //SUMMARY:
            //| Operator                   | Description                         | Output Example           |
            //| -------------------------- | ----------------------------------- | ------------------------ |
            //| **Range(start, count) * *  | Generates consecutive numbers       | Range(5, 3) → 5,6,7      |
            //| **Repeat(value, count) * * | Repeats a value                     | Repeat("X", 4) → X,X,X,X |
            //| **Empty<T>() * *           | Returns empty sequence              | Empty<Student>()         |
            //| **Append(item) * *         | Adds element at end                 | List + item              |
            //| **Prepend(item) * *        | Adds element at start                | item + List             |
            //| **Zip(seq1, seq2) * *      | Combines two sequences element - wise | (A1, B1), (A2, B2)…    |

            #endregion




            #region Conversion Operators
            Console.WriteLine("-----LINQ Conversion Operators------");
            Console.WriteLine("----ToList()---");
            //Converts a sequence into a List<T>.
            //Why use it:
            //To materialize(execute) a LINQ query
            //To modify the result(because List is editable)
            List<int> numbersE = new List<int> { 1, 2, 3, 4, 5 };
            var list = numbersE.Where(n => n > 2).ToList();
            Console.WriteLine($"List: {string.Join(", ", list)}"); // Output: List: 3, 4, 5

            //Example with filtering
            var csStudents1 = students
                    .Where(s => s.DepartmentId == 1)
                    .ToList();



            Console.WriteLine("----ToArray()---");
            //Converts a sequence into a T[] array.
            //✔ Why use it:
            //When you need fixed-size collection
            //When working with APIs requiring arrays
            List<int> numbersF = new List<int> { 1, 2, 3, 4, 5 };
            var array = numbersF.Where(n => n > 2).ToArray();
            Console.WriteLine($"Array: {string.Join(", ", array)}"); // Output: Array: 3, 4, 5

            var studentArray = students.ToArray();    //Now studentArray is an array of Student.


            Console.WriteLine("-------ToDictionary()-------");
            //Converts a sequence into a Dictionary<TKey, TValue>.
            //✔ Required:
            //A key selector
            //Optionally a value selector
            var studentDict = students.ToDictionary(s => s.Id);
            //| Key(Id) | Value(Student) |
            //| -------- | --------------- |
            //| 1       | Amit |
            //| 2       | Sneha |
            //| 3       | Rahul |
            //| 4       | Priya |
            //| 5       | Kiran |

            //Key = Name, Value = Age
            var nameAgeDict = students.ToDictionary(
                    s => s.Name,
                    s => s.Age
                 );


            Console.WriteLine("-------Cast<T>()------");
            //Casts each element in a non-generic collection to type T.
            //✔ Used mainly with:
            //ArrayList
            //IEnumerable that is not of specific type
            ArrayList list4 = new ArrayList { 1, 2, 3 };
            var integers = list4.Cast<int>();                  //now numbers is an IEnumerable<int>.
            Console.WriteLine(string.Join(", ", integers)); // Output: 1, 2, 3

            //ArrayList arr = new ArrayList { "A", 100, true }; //If casting fails → exception
            //var num = arr.Cast<int>();  // ERROR at runtime


            //SUMMARY:
            //| Operator             | Converts To              | Editable ?    | Notes |
            //| ------------------   | -----------------------  | ------------ | ------------------------------------ |
            //| **ToList() * *       | List<T>                  | ✔ Yes         | Used to execute query + modify data |
            //| **ToArray() * *      | T[] array                | ❌ Fixed size | Best for APIs requiring arrays |
            //| **ToDictionary() * * | Dictionary<TKey, TValue> | ✔ Yes         | Key must be unique |
            //| **Cast<T>() * *      | IEnumerable<T>           | Sequence       | Converts from non-generic collection |




            //Difference Between LINQ Cast and OfType
                // Cast: Attempts to cast all elements to the specified type, throwing an exception if any cast fails.
                // OfType: Filters elements that can be cast to the specified type, ignoring others.
            #endregion






            //Conclusion:
            //LINQ is a versatile and powerful feature in C# that simplifies data querying across various sources. Its
            //architecture, built on providers, extension methods, and expression trees, supports both in-memory
            //(IEnumerable<T>) and remote (IQueryable<T>) queries. The wide range of operators—filtering (Where, OfType),
            //projection (Select, SelectMany), set operations (Distinct, Union), ordering (OrderBy, ThenBy), aggregates (Sum,
            //Max), quantifiers (All, Any), joins, and more—enables complex data manipulation with concise syntax. Deferred
            //execution optimizes performance, while immediate execution operators like ToList provide flexibility. The
            //examples provided cover basic to advanced scenarios, demonstrating LINQ’s applicability in real-world
            //applications like customer order processing.

        }

    }
}
