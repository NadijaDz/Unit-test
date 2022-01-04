# Jap final task

Usage Normative Calculator Application

-You need to do the few things listed below before you can properly start the application.

-After you cloned the project, open the folder “NormativeCalculatorAPI ”, open the .sln , and in Package Manager Console choose as Default project: NormativeCalculator.Database and type
“Update-database -Context NormativeCalculatorDBContext” to seed data into a database and also type 
"Update-database -Context NormativeCalculatorLoggerDBContext" for add logging.

After that, start the project with (F5, ctrl F5). That project is API and on the startup, it will be Swagger where you need to authorized for some endpoints. You can do that on click button Authorize and Login with Google Account.

-After you started the Web API project, now you can open the folder “NormativeCalculatorUI/client”. Open this folder in Visual Studio Code. Open Terminal and use the command below for
installing workspace npm dependencies:


    npm install


-After the installation of all required npm dependency packages, for starting application in the same Terminal type command:


    ng serve


-It will provide URL: http://localhost:4200/ copy that URL and past it into a new tab in Browser

-Browser will open application and if you want to use application you need to login with your Google Account.



# About application

-This is application for calculated  price for ingredients of recipe. When user is logged, on initial screen can see all categories of recipes.
-User can click on category, and when click, will see new page with all recipes of selected category. On this page you can search recipes
 by recipe name, recipe description and ingredient name. Also you can add new recipe on selected category, and can add attribute for recipe like:
name, description, ingredient with selected ingredient name, entered quantity of ingredient, and selected measure unit. 
Application immediately calculated all entered value for ingredient and show how new recipe will be cost. 
-User also can see details about recipe when click on button View Details next to each recipe.


# Postman Collection Test

Import Normative Calculator.postman_collection.json into your Postman and test endpoints. 
For test endpoints you need auth_cookie from browser.
You get this auth_cookie from Swagger after you login with Google account or via Frontend NormativeCalculator application where you need open developer console. Click on  application tab and under you can see the cookies and you can find cookies with name auth_cookie. You wiil need to copy value of cookie auth_cookie and return on Postman and go to  NormativeCollection tab Variables and and set as variable name of cookie for example: cookie and past in current value, value from frontend. 
If request which you want to test need a autorization you go on tab Headers  and set cookie from Collection where as Key you can add Cookie and as Value you put in {{}}  your value from variable {{cookie}} in NormativeCollection. And than you can test request.

