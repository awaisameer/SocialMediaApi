# SocialMediaApp
🌐 Technologies Used:
Backend: .NET Core for a high-performance, cross-platform API.
Database: MongoDB for flexible and scalable data storage.
Security: JWT Authentication, bcrypt for password hashing, and role-based authorization to keep your data safe.
⚙️ Key Features:
User Authentication & Authorization:
Users can register, log in, and receive a JWT token for secure access to protected routes. Only authenticated users can share, delete, or update posts.

CRUD Operations for Posts:
Users can create, update, delete, and read posts. Each post is linked to the user who created it, ensuring personalized content management.

User Profile Management:
Users can create their profiles with essential details like first name, last name, email, phone number, country, city, etc. All fields are validated for smooth data entry.

MongoDB Integration:
MongoDB is used for its NoSQL flexibility, supporting dynamic and scalable data structures, especially for storing user profiles and posts.

Post Visibility:
All posts are linked to the user’s unique ID, and MongoDB’s powerful query system makes it easy to fetch posts based on user ID.

JWT Token:
JWT tokens are issued during login, ensuring that users can securely interact with the API and perform actions based on their roles (admin/user).

🔒 Security Features:
Password Hashing:
Passwords are securely hashed using bcrypt before storing them in the database to prevent data breaches.

JWT Authentication:
JWT ensures that only authenticated users can make post-related requests, offering an added layer of security.

Role-Based Access Control:
Posts can be managed only by the user who created them, ensuring that unauthorized users cannot modify someone else’s content.

📊 API Architecture:
MVC Design Pattern:
The API follows the Model-View-Controller pattern to separate concerns, making the code modular and easier to maintain.

MongoDB Service Layer:
The service layer handles interactions with MongoDB, offering an abstraction to access user data, posts, and more.

POSTMAN Testing:
Postman is used for testing various API endpoints like user registration, login, post creation, and updates, ensuring that each feature works as expected.

💡 Advantages:
Scalability: MongoDB allows for horizontal scaling, making it easy to handle a growing number of users and posts.
Security: JWT Authentication ensures that users' data is safe, and role-based access adds an extra layer of protection.
Flexibility: MongoDB’s schema-less structure allows easy modifications in the data model without affecting the API’s performance.
🔧 What’s Next?
Real-Time Notifications: Implement real-time messaging or notifications for a more interactive user experience.
Advanced Analytics: Add features for post analytics, likes, and comments for social engagement.
