# Service API built with .NET 5.0 and RavenDB

Resume:

An API that allows users to add comments related with some entity.

Important notes:

1) To run this project you'll need a valid certificate to access my database. Once you have this, it should be placed inside the [certificates' folder](https://github.com/joao-figueira/MainApplicationService/tree/master/MainApplicationService.Api/Certificates), and be named as "SuperCoolNoSQLdbCertificate.pfx". If you don't want to follow these steps, you should create your own RavenDB database and replace all [RavenSettings (on appsettings.json)](https://github.com/joao-figueira/MainApplicationService/blob/master/MainApplicationService.Api/appsettings.json).
2) The authentication process is outside the scope of this project (for now). Having this is mind, you should add a "UserId" header on all api requests. It will be processed by a [custom middleware](https://github.com/joao-figueira/MainApplicationService/blob/master/MainApplicationService.Api/Middlewares/CurrentUserMiddleware.cs), and set a ficticious user to be used by the service.
3) The file ["OffensiveExpressions.txt"](https://github.com/joao-figueira/MainApplicationService/blob/master/MainApplicationService/OffensiveExpressions.txt) contains a list of expressions (separated by ';') which we consider to be offensive and we don't want the users to write on comments.
4) The [ArticleController.cs](https://github.com/joao-figueira/MainApplicationService/blob/master/MainApplicationService.Api/Controllers/ArticleController.cs) was added just for help the tests of the comments' api. To be more specific: it only has 1 endpoint to list the available articles to comment). All the focus should be around the code related with the comments.

### How to use this API:

**1) GET "/api/v1/comments/{commentId}" - Get a comment by ID**

    QueryParams:

    - None

    Returns:

    - HttpStatusCode OK (200) with the CommentDto
    - HttpStatusCode NotFound (404) if the commentId doesn't belong to an existing comment
    - HttpStatusCode Forbidden (403) if the user doesn't has permissons to access the comment

    If the commentId is actually an existing id of a comment, it returns a http Ok result with the CommentDto. Else, it return a NotFound result.

    It also marks the comment as read by the user (userId passed on the header).
  
**2) GET "/api/v1/entity/{entityId}/comments" - Get the list of comments for a specific entity (e.g. an article)**
   
    QueryParams:

    - skip: (int) - Optional - Default: 0 - Skip the first X results.
    - take: (int) - Optional - Default: 50 - Take only X results. 
    - onlyNew: (boolean) - Optional - Default: false - True to only return comments not seen by the current user (passed the header "UserId").

    Returns:

    - HttpStatusCode OK (200) with the CommentDto
    - HttpStatusCode NotFound (404) if the parent entity id doesn't belong to an existing entity (for example, an article)
    - HttpStatusCode Forbidden (403) if the user doesn't has permissons to access the parent entity, for example


    This requests marks all the comments returned as seen by the user.

  
**3) POST "/api/v1/entity/{entityId}/comments" - Create a new comment for a specific entity (e.g. an article)** 

    QueryParams:

    - None

    Body:
    
      {
          "text": "string"
      }

    Returns:

    - HttpStatusCode Created (201) with the CommentDto
    - HttpStatusCode NotFound (404) if the parent entity id doesn't belong to an existing entity (for example, an article)
    - HttpStatusCode Forbidden (403) if the user doesn't has permissons to access the parent entity, for example
    - HttpStatusCode BadRequest (400) if the postModel is not valid (for example, if the text contains a offensive expression)
    
      
**4) PUT "/api/v1/comments/{commentId}" - Update a comment by id**

    QueryParams:

    - None

    Body:
    
      {
          "text": "string"
      }

    Returns:

    - HttpStatusCode Ok (200) with the CommentDto
    - HttpStatusCode NotFound (404) if the parent entity id doesn't belong to an existing entity (for example, an article)
    - HttpStatusCode Forbidden (403) if the user doesn't has permissons to access the parent entity, for example
    - HttpStatusCode BadRequest (400) if the postModel is not valid (for example, if the text contains a offensive expression)
    - HttpStatusCode Conflict (409) if in the meanwhile, the comment was updated or deleted by another user.

**5) DELETE "/api/v1/comments/{commentId}" - Delete a comment by id**

    QueryParams:

    - None

    Returns:

    - HttpStatusCode Ok (200) with the CommentDto
    - HttpStatusCode NotFound (404) if the parent entity id doesn't belong to an existing entity (for example, an article)
    - HttpStatusCode Forbidden (403) if the user doesn't has permissons to access the parent entity, for example
    - HttpStatusCode BadRequest (400) if for some reason (business rule) the comment can't be deleted, for example
    - HttpStatusCode Conflict (409) if in the meanwhile, the comment was updated or deleted by another user.

### Testing the API with POSTMAN

You can import the [postman collection](https://github.com/joao-figueira/MainApplicationService/blob/master/Postman/Main%20App%20Tests.postman_collection.json) to test the api.

There you have 7 different requests:

1) "GET ALL ARTICLES" - A request to get the existing articles on the database. This API call will return a simple dto, with multiple articles and respective ids, that you'll use to post a comment or get all the comments associated with it. This endpoint is present just to let you know the ids.
2) "GET ALL ENTITY COMMENTS" - Using one id from the previous request, you can get all the existing comments for that entity with skip and take.
3) "GET NEW ENTITY COMMENTS" - It's actually very similiar to the previous one, except for the fact that this one has an extra parameter ("onlyNew"). This parameter says to the service that you are only interested on the comments that you haven't seen before. So, if you perform this call 2 times in a row with the same UserId (it's a header, don't forget it) you'll get 2 different results: the second time you won't be receiving the same results since you seen it before (on the first time).
4) "GET A COMMENT BY ID" - Returns the CommentDto, if exists. Not much to say about this one.
5) "CREATE COMMENT" - Again, with an ID from the request number one, you can add a comment to an article (or another existing entity). After this, you should be able to get it with the requests number 2, 3 and 4 (if you use the same parent entity ID, of course).
6) "DELETE COMMENT" - Deletes a comment by ID, if exists. 
7) "UPDATE COMMENT" - Updates a comment by ID, if exists.
