# CommentsAPI_RavenDB

.NET 5.0 WebAPI with RavenDB 

Resume:

An API that allows users to add comments related with some entity.

Important notes:

1) To run this project you'll need a valid certificate to access my database. Once you have this, it should be placed inside the certificates' folder, and be named as "SuperCoolNoSQLdbCertificate.pfx". If you don't want to follow these steps, you should create your own RavenDB database and replace all RavenSetting (on appsettings.json).
2) The authentication process is outside the scope of this project (for now). Having this is mind, you should add a "UserId" header on all api requests. It will be processed by a custom middleware, and set a ficticious user to be used by the service.
3) The main directory contains a file named "OffensiveExpressionsList.txt". Inside this file we can set a list of expressions (separated by ';') which we consider to be offensive and we don't want the users to write on comments.

How to use this API:

1) Get a comment by id: GET ".../api/v1/comments/{commentId}"
  
    If the commentId is actually an existing id of a comment, it returns a http Ok result with the CommentDto. 

    Else, it return a NotFound result.

    It also marks the comment as read by the user (userId passed on the header).
  
2) Get the list of comments of a specific entity: GET ".../api/v1/entity/{entityId}/comments"

    Params:

      - skip: (int) - optional - skip the first X results. Default: 0

      - take: (int) - optional - return X results. Default: 50

      - onlyNew: (boolean) - optional - true to only return comments not seen by the current user. Default: false
  
3) Create a new comment for a specific entity: POST ".../api/v1/entity/{entityId}/comments"
    
      Model: 
      ```json
      {
          "text": "string"
      }
      ```
4) Update a comment by id: PUT ".../api/v1/comments/{commentId}"

      Model: 
      ```json
      {
          "text": "string"
      }
      ```

5) Delete a comment by id: DELETE ".../api/v1/comments/{commentId}"
