using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.Json;
using VyneSDETTakeHome.Models;
using VyneSDETTakeHome.TestMethods;

namespace VyneSDETTakeHome.TestCases
{
   /// <summary>
   /// This tests the JSON API against jsonplaceholder.typicode.com
   /// </summary>
   /// 
   [TestFixture]
   public class JSONPlaceHolder : BaseTest
   {

      [TestCase("P1_GetPosts_Returns200OK_AndValidList")]
      public async Task P1_GetPosts_Returns200OK_AndValidList(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         string baseUrl = JsonPlaceholderBaseUrl;
         string endpoint = $"{baseUrl.TrimEnd('/')}/posts";

         // 2. Initialize APIRequestContext pointing to JsonPlaceholderBaseUrl/posts.
         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 3. Send GET request to retrieve all posts.
         DebugOutput($"Sending GET request to: {endpoint}");
         var response = await apiContext.GetAsync("/posts");

         // 4. Assert response status code is 200 (OK).
         Assert.That(response.Status, Is.EqualTo(200), $"Expected status 200 OK but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status} OK");

         // 5. Deserialize response body into a list of Post models.
         string jsonBody = await response.TextAsync();
         var posts = JsonSerializer.Deserialize<List<Post>>(jsonBody, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });

         Assert.That(posts, Is.Not.Null, "Failed to deserialize JSON response into Post list.");

         // 6. Assert that the post collection is not empty and contains expected total count (100 posts).
         Assert.That(posts, Is.Not.Empty, "Expected posts list to contain items.");
         Assert.That(posts!.Count, Is.EqualTo(100), $"Expected 100 posts, but received {posts.Count}");
         DebugOutput($"Total Posts Returned: {posts.Count}");

         // 7. Verify first post object contains valid id, userId, non-empty title, and non-empty body.
         var firstPost = posts.First();
         Assert.That(firstPost.Id, Is.GreaterThan(0), "Expected first post Id to be greater than 0.");
         Assert.That(firstPost.UserId, Is.GreaterThan(0), "Expected first post UserId to be greater than 0.");
         Assert.That(firstPost.Title, Is.Not.Null.And.Not.Empty, "Expected first post Title to not be empty.");
         Assert.That(firstPost.Body, Is.Not.Null.And.Not.Empty, "Expected first post Body to not be empty.");

         DebugOutput($"First Post Verified -> ID: {firstPost.Id}, UserId: {firstPost.UserId}, Title: '{firstPost.Title.Substring(0, 20)}...'");

         // 8. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P1_GetPostById_Returns200OK_AndMatchingPostData")]
      public async Task P1_GetPostById_Returns200OK_AndMatchingPostData(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 2. Set target postId parameter.
         int targetPostId = 1;
         string baseUrl = JsonPlaceholderBaseUrl;

         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 3. Send GET request to JsonPlaceholderBaseUrl/posts/{postId}.
         DebugOutput($"Sending GET request to: {baseUrl.TrimEnd('/')}/posts/{targetPostId}");
         var response = await apiContext.GetAsync($"/posts/{targetPostId}");

         // 4. Assert response status code is 200 (OK).
         Assert.That(response.Status, Is.EqualTo(200), $"Expected status code 200 OK, but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status} OK");

         // 5. Deserialize response body into a single Post model.
         string jsonBody = await response.TextAsync();
         var post = JsonSerializer.Deserialize<Post>(jsonBody, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });

         Assert.That(post, Is.Not.Null, "Failed to deserialize JSON response into Post model.");

         // 6. Assert that returned post id matches expected target postId (1).
         Assert.That(post!.Id, Is.EqualTo(targetPostId), $"Expected Post ID to be {targetPostId}, but got {post.Id}");

         // 7. Assert that userId, title, and body fields are not null or empty.
         Assert.That(post.UserId, Is.GreaterThan(0), "Expected UserId to be greater than 0.");
         Assert.That(post.Title, Is.Not.Null.And.Not.Empty, "Expected Title to not be null or empty.");
         Assert.That(post.Body, Is.Not.Null.And.Not.Empty, "Expected Body to not be null or empty.");

         DebugOutput($"Verified Post Details -> ID: {post.Id}, UserId: {post.UserId}, Title: '{post.Title.Substring(0, 20)}...'");

         // 8. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }


      [TestCase("P1_CreatePost_Returns201Created_AndNewPostData")]
      public async Task P1_CreatePost_Returns201Created_AndNewPostData(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         string baseUrl = JsonPlaceholderBaseUrl;

         // 2. Construct new Post payload object (title, body, userId).
         var newPostPayload = new Post
         {
            UserId = 1,
            Title = "Automated SDET Test Post Title",
            Body = "This is the body content created during play-wright automated test execution."
         };

         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 3. Send POST request to JsonPlaceholderBaseUrl/posts with serialized JSON body.
         DebugOutput($"Sending POST request to: {baseUrl.TrimEnd('/')}/posts");
         var response = await apiContext.PostAsync("/posts", new APIRequestContextOptions
         {
            DataObject = newPostPayload
         });

         // 4. Assert response status code is 201 (Created).
         Assert.That(response.Status, Is.EqualTo(201), $"Expected status 201 Created, but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status} Created");

         // 5. Deserialize response body into a created Post model.
         string jsonBody = await response.TextAsync();
         var createdPost = JsonSerializer.Deserialize<Post>(jsonBody, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });

         Assert.That(createdPost, Is.Not.Null, "Failed to deserialize JSON response into Post model.");

         // 6. Assert that the generated id field is returned in response (JSONPlaceholder mocks new IDs as 101).
         Assert.That(createdPost!.Id, Is.GreaterThan(0), "Expected generated Post Id to be greater than 0.");
         DebugOutput($"Generated Post ID: {createdPost.Id}");

         // 7. Assert that title, body, and userId in response match payload input values.
         Assert.That(createdPost.UserId, Is.EqualTo(newPostPayload.UserId), "UserId in response does not match request payload.");
         Assert.That(createdPost.Title, Is.EqualTo(newPostPayload.Title), "Title in response does not match request payload.");
         Assert.That(createdPost.Body, Is.EqualTo(newPostPayload.Body), "Body in response does not match request payload.");

         DebugOutput($"Verified Created Post -> ID: {createdPost.Id}, Title: '{createdPost.Title}'");

         // 8. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P2_UpdatePost_Returns200OK_AndUpdatedPayloadData")]
      public async Task P2_UpdatePost_Returns200OK_AndUpdatedPayloadData(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 2. Set target postId parameter.
         int targetPostId = 1;
         string baseUrl = JsonPlaceholderBaseUrl;

         // 3. Construct updated Post payload object with modified title and body values.
         var updatedPostPayload = new Post
         {
            Id = targetPostId,
            UserId = 1,
            Title = "Updated Post Title - SDET Automation",
            Body = "This body text has been replaced via a PUT request during test execution."
         };

         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 4. Send PUT request to JsonPlaceholderBaseUrl/posts/{postId} with updated JSON body.
         DebugOutput($"Sending PUT request to: {baseUrl.TrimEnd('/')}/posts/{targetPostId}");
         var response = await apiContext.PutAsync($"/posts/{targetPostId}", new APIRequestContextOptions
         {
            DataObject = updatedPostPayload
         });

         // 5. Assert response status code is 200 (OK).
         Assert.That(response.Status, Is.EqualTo(200), $"Expected status 200 OK, but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status} OK");

         // 6. Deserialize response body into updated Post model.
         string jsonBody = await response.TextAsync();
         var updatedPost = JsonSerializer.Deserialize<Post>(jsonBody, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });

         Assert.That(updatedPost, Is.Not.Null, "Failed to deserialize JSON response into Post model.");

         // 7. Assert that post id matches target postId (1).
         Assert.That(updatedPost!.Id, Is.EqualTo(targetPostId), $"Expected Post ID to be {targetPostId}, but got {updatedPost.Id}");

         // 8. Assert that title and body reflect the updated payload values.
         Assert.That(updatedPost.Title, Is.EqualTo(updatedPostPayload.Title), "Title in response does not match updated payload.");
         Assert.That(updatedPost.Body, Is.EqualTo(updatedPostPayload.Body), "Body in response does not match updated payload.");

         DebugOutput($"Verified Updated Post -> ID: {updatedPost.Id}, Title: '{updatedPost.Title}'");

         // 9. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P2_DeletePost_Returns200OK")]
      public async Task P2_DeletePost_Returns200OK(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 2. Set target postId parameter to delete.
         int targetPostId = 1;
         string baseUrl = JsonPlaceholderBaseUrl;

         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 3. Send DELETE request to JsonPlaceholderBaseUrl/posts/{postId}.
         DebugOutput($"Sending DELETE request to: {baseUrl.TrimEnd('/')}/posts/{targetPostId}");
         var response = await apiContext.DeleteAsync($"/posts/{targetPostId}");

         // 4. Assert response status code is 200 (OK) or 204 (No Content).
         Assert.That(response.Status, Is.EqualTo(200).Or.EqualTo(204), $"Expected status 200 OK or 204 No Content, but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status}");

         // 5. Verify response body payload is empty object `{}`.
         string jsonBody = await response.TextAsync();
         string cleanedResponseBody = jsonBody.Trim();

         Assert.That(cleanedResponseBody, Is.EqualTo("{}").Or.EqualTo(string.Empty), "Expected empty JSON object or empty string response for DELETE request.");
         DebugOutput($"Verified Response Body: '{cleanedResponseBody}'");

         // 6. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P3_GetPost_InvalidId_Returns404NotFound")]
      public async Task P3_GetPost_InvalidId_Returns404NotFound(string testName)
      {
         // 1. Log test execution start and inTestCaseID header.
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 2. Set invalid non-existent postId parameter.
         int invalidPostId = 99999;
         string baseUrl = JsonPlaceholderBaseUrl;

         var apiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
         {
            BaseURL = baseUrl
         });

         // 3. Send GET request to JsonPlaceholderBaseUrl/posts/{invalidPostId}.
         DebugOutput($"Sending GET request to non-existent endpoint: {baseUrl.TrimEnd('/')}/posts/{invalidPostId}");
         var response = await apiContext.GetAsync($"/posts/{invalidPostId}");

         // 4. Assert response status code is 404 (Not Found).
         Assert.That(response.Status, Is.EqualTo(404), $"Expected status 404 Not Found, but received {response.Status}");
         DebugOutput($"Response Status Code: {response.Status} Not Found");

         // 5. Log test completion and call DebugOutput("**** TEST PASSED").
         DebugOutput("**** TEST PASSED");
      }
   }
}
