// Users Service
-- Sign Up
-- Sign In

// Users
- Admins
- Customers

// Orders
GET orders/carts /{cart_id}
Gets list of items in a cart (cart_id is a uuid, generated and stored the client side)

POST orders/carts/{cart_id}
Takes object of event_id, seat_id and price_id as a payload and adds a seat to the cart. Returns a cart state (with total amount) back to the caller)

DELETE orders/carts/{cart_id}/events/{event_id}/seats/{seat_id}
Deletes a seat for a specific cart

// Payment Service
GET payments/{payment_id}
Returns the status of a payment

POST payments/{payment_id}/complete
Updates payment status and moves all the seats related to a payment to the sold state.

POST payments/{payment_id}/failed
Updates payment status and moves all the seats related to a payment to the available state.