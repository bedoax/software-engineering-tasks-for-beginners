## Task 6: CRUD for Posts with Sync Command

### Objective
Create a CRUD system for posts with a command line tool to sync posts from an external API and approval by users with the "review" role.

### Requirements
1. Use a web framework (e.g., Express for Node.js, Flask for Python).
2. Implement the following endpoints (accessible only to logged-in users):
   - `POST /posts`: Create a new post.
   - `GET /posts`: Retrieve a list of all approved posts (public).
   - `GET /posts/all`: Retrieve a list of all posts (Admin, Reviewer).
   - `GET /posts/:id`: Retrieve a specific approved post by ID.
   - `PUT /posts/:id`: Update a specific post by ID.
   - `DELETE /posts/:id`: Delete a specific post by ID.
   - `POST /posts/:id/approve`: Approve a post (Review role).
3. Implement a command line tool to sync posts from `https://jsonplaceholder.typicode.com/posts`:
   - Insert all posts if they do not exist.
4. Only users with the "review" role can approve posts to be shown in the public `GET /posts` endpoint.
5. Use a database to store post information (e.g., MongoDB, PostgreSQL).
6. Validate input data to ensure data integrity.
7. Handle errors gracefully and return appropriate HTTP status codes.
8. Seed the database with roles and users:
   - Roles: Admin, Reviewer.
   - Users: One user with Admin role, one user with Reviewer role.

### Example
Here is an example of how the API endpoints should work:

- **Create a new post**
  ```
  POST /posts
  {
    "title": "New Post",
    "body": "This is the content of the new post."
  }
  ```

- **Retrieve all approved posts (public)**
  ```
  GET /posts
  ```

- **Retrieve all posts (Admin, Reviewer)**
  ```
  GET /posts/all
  ```

- **Retrieve a specific approved post by ID**
  ```
  GET /posts/1
  ```

- **Update a specific post by ID**
  ```
  PUT /posts/1
  {
    "title": "Updated Post",
    "body": "This is the updated content of the post."
  }
  ```

- **Delete a specific post by ID**
  ```
  DELETE /posts/1
  ```

- **Approve a post (Review role)**
  ```
  POST /posts/1/approve
  ```

### Command Line Tool
- **Sync posts from external API**
  ```
  node syncPosts.js
  ```

### Authorization Logic 
- **Admin**: Can create, read, update, and delete any post.
- **Reviewer**: Can approve posts to be shown in the public `GET /posts` endpoint.
- **Logged-in Users**: Can access the endpoints.

### Seeder
- **Roles**
  ```
  {
    "roles": [
      { "name": "Admin" },
      { "name": "Reviewer" }
    ]
  }
  ```

- **Users**
  ```
  {
    "users": [
      { "username": "admin", "password": "admin123", "role": "Admin" },
      { "username": "reviewer", "password": "reviewer123", "role": "Reviewer" }
    ]
  }
  ```