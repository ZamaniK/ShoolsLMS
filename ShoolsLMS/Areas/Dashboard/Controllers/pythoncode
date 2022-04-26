rate = float(input("Enter Rate : "))
quantity = float(input("Enter Quantity : "))

amount = rate*quantity
print("Amount = ",amount)

if(amount >= 100000):
    discount = 20
elif(amount>=50000 and amount<=99999):
   discount = 10
else:
   discount = 5

discountAmount = (amount*discount)/100
netAmt = amount - discountAmount

print("Discount = %d%% and Discount Amount = %.2f"%(discount,discountAmount))

print("Net Amount = ",netAmt)
