# Restaurant Application Project : WIP

## Introduction

A table booking and food ordering app made with ASP.Net and JS.
The app contains the interface for admin as well as the public from the same port itself.

## Workflow

The app is still in its alpha state, with bugs and security vulnerabilities as well.

1. For users who have not booked the table.

   1. User
      1. Once the app is up, access the Menu option in the Navbar.
      1. Using the + and - buttons provided select the quantity of the items you want.
      1. Click on Order! button at the bottom to place the order.
      1. Once the order is placed, the user will be redirected to a status page where they can see the
         OrderID, Time at which the order was placed and the current status of the Order.
   1. Admin
      1. Once the user places the order, Admin can use the Manage link in Navbar to see the order.
      1. The new order will be shown under the link, Orders.
      1. Select the new order from the list of all orders. It'll be in the bottom by default. (Sorting is WIP).
      1. Admin can change the order status from Placed to any of the appropriate selection from the drop down list.
      1. Once the option is selected, admin should Save the changes.
   1. User
      1. User can check the updated status change of the order by reloading the page. (AJAX WIP)

1. For users who have booked the table.

   1. User
      1. Once the app is up, navigate to Book Table in the Navbar.
      1. Enter the number of customers who will be visiting the restaurant, any allergies, and time and date of booking. (Calender and Clock will be added soon)
      1. Please enter the bookingDateTime in this format : 11/06/2021 12:00:00
      1. Once the booking is registered, the user will be redirected to a status page where they can check the status of the order.
   1. Admin
      1. Manage > Booking
      1. Admin will be able to see all the bookings, and the current booking will be shown with a status as Pending.
      1. Admin can click on the Edit page to allote a table to the customer, as well as change the status to Confirmed/Cancelled.
   1. User
      1. Once admin confirms the booking, the text entry in the user's status page will change to an Order Food button.
      1. User clicks on the Order Button, and a hidden form field is sent to the landing page containing the booking ID as well.
      1. This hidden bookingID will be passed to the backend once the order is placed by the user.
      1. Remaining user interaction is same as mentioned in section (1)

1. Admin Exclusive Features
   1. Admins can add, edit, remove any Food Item from the list.
   2. Almost all CRUD functionalities for the same is provided now.

## Note
- For the api-user to fetch the data make sure your DB contains a user, and the user's detail is added to Client.cs under Models
   
## Screenshots

### Desktop
- ![Desktop Main Page](screenshots/desktop_1.png | width=100)

### Mobile

## Things to Add

- [ ] Once the order is delivered/served, the user will be presented with a page to rate the food.
- [ ] This review is appended to the existing item's rating.
- [ ] Once an order is completed the Order Count of the item will be incremented by 1.
- [ ] Authorize users.

## Known Bugs

~~BookingDateTime shows 01-01-0001 00:00:00 after the customer books the table~~

## BugFixes

- Order button not hiding once the user scrolls to the bottom : Fixed

## Updates

- 15-06-2021 : Admin modules are reachable through /Admin link, Manage options removed from User facing Layout.
- 21-06-2021 : UI updated, Payment with RazorPay integrated, Google Maps API integrated.
- 23-06-2021 : Web-API token authentication added.
- 24-06-2021 : FoodController and BookingsController are using http-client for fetching data.
- 24-06-2021 : Full CRUD of Bookings implemented with httpClient
