let deleteButtons = document.querySelectorAll(".deleteBtnImage");

deleteButtons.forEach(btn => btn.addEventListener("click", function(){
    btn.parentElement.remove()
}))


//Delete sopping item

let deleteSoppingItem = document.querySelectorAll(".deleteCardItem");

deleteSoppingItem.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    fetch(url)
        .then(response => {
            if (response.status == 200) {
                btn.parentElement.parentElement.remove()
            }
            else {
                alert("Unknown Error!")
            }
        })
}))


//Delete favourite item

let deleteFavouriteItem = document.querySelectorAll(".deleteFavouriteItem");

deleteFavouriteItem.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    fetch(url)
        .then(response => {
            if (response.status == 200) {
                btn.parentElement.parentElement.parentElement.remove()
            }
            else {
                alert("Unknown Error!")
            }
        })
}))




//Add to basket

let addToBasketBtns = document.querySelectorAll(".basketaddbtn");

addToBasketBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    url = btn.getAttribute("href")
    fetch(url).then(res => {
        if (res.status == 200) {
            Toastify({
                text: "Added to shopping cart",
                gravity: "bottom",
                duration: 3000,
                style: {
                    background: "green"
                }
            }).showToast();

            fetch("/home/getbasket").then(response => response.text())
                .then((html) => {
                    block = document.querySelector(".basket-items")
                    block.innerHTML = html
                    span = document.querySelector(".countSpan")
                    spanPrice = document.querySelector(".totalPrice")
                    spanPrice2 = document.querySelector(".totalPrice2")
                    fetch("/home/getbasketcount")
                        .then(response => response.json())
                        .then(res => {
                            console.log(res)
                            span.innerHTML = res.count;
                            
                            const formattedPrice = res.price.toFixed(2);
                            const displayPrice = formattedPrice.padStart(7, "0");
                            
                            spanPrice.innerHTML = displayPrice;
                            spanPrice2.innerHTML = displayPrice;
                        })
                    block.querySelectorAll(".btn-deleteproduct").forEach((btn, i) => btn.addEventListener("click", function (e) {
                        e.preventDefault();
                        fetch(btn.getAttribute("href")).then(response => response.json()).then(response => {
                            Toastify({
                                text: "Deleted from shopping cart",
                                gravity: "bottom",
                                duration: 3000,
                                style: {
                                    background: "red"
                                }
                            }).showToast();
                            console.log(response[i])
                            if (response[i].count <= 0) btn.parentElement.remove()
                            else {
                                btn.parentElement.querySelector("#basketItemCount").innerHTML = response[i].count
                            }
                            span = document.querySelector(".countSpan")
                            spanPrice = document.querySelector(".totalPrice")
                            spanPrice2 = document.querySelector(".totalPrice2")
                            fetch("/home/getbasketcount")
                                .then(response => response.json())
                                .then(res => {
                                    //console.log(res)
                                    let displayPriceForDelete;
                                    if (res.price === 0) {
                                        displayPriceForDelete = "0.00"
                                    }
                                    else {
                                        const formattedPriceForDelete = res.price.toFixed(2);
                                        displayPriceForDelete = formattedPriceForDelete.padStart(7, "0");
                                    }
                                    
                                    span.innerHTML = res.count;
                                    spanPrice.innerHTML = displayPriceForDelete;
                                    spanPrice2.innerHTML = displayPriceForDelete;
                                })
                            fetch("/home/cleanbasket")
                                .then(response => {
                                    if (res.status == 200) {
                                        console.log("deleted")
                                    }
                                    else {
                                        console.log("error")
                                    }
                                })
                        })
                    }))
                })
            
        }
        else alert("Unknown error")
    })
}))



//delete card item
let deleteBtns = document.querySelectorAll(".btn-deleteproduct");

deleteBtns.forEach((btn, i) => btn.addEventListener("click", function (e) {
    e.preventDefault();
    fetch(btn.getAttribute("href")).then(response => response.json()).then(response => {
        Toastify({
            text: "Deleted from shopping cart",
            gravity: "bottom",
            duration: 3000,
            style: {
                background: "red"
            }
        }).showToast();
        console.log(response[i])
        //if(response.count == null) alert("Alert")
        if (response[i].count <= 0) btn.parentElement.remove()
        else {
            btn.parentElement.querySelector("#basketItemCount").innerHTML = response[i].count
        }
        span = document.querySelector(".countSpan")
        spanPrice = document.querySelector(".totalPrice")
        spanPrice2 = document.querySelector(".totalPrice2")
        fetch("/home/getbasketcount")
            .then(response => response.json())
            .then(res => {
                console.log(res)
                let displayPriceForDelete;
                if (res.price === 0) {
                    displayPriceForDelete = "0.00"
                }
                else {
                    const formattedPriceForDelete = res.price.toFixed(2);
                    displayPriceForDelete = formattedPriceForDelete.padStart(7, "0");
                }
                
                span.innerHTML = res.count;
                spanPrice.innerHTML = displayPriceForDelete;
                spanPrice2.innerHTML = displayPriceForDelete
            })
        
    })


}))



    


//Delete Btns

let itemDeleteBtns = document.querySelectorAll(".delete-item-btn")

itemDeleteBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            let url = btn.getAttribute("href");
            fetch(url)
                .then(res => {
                    if (res.status == 200) {
                        window.location.reload(true)
                    }
                    else {
                        alert("Item tapilmadi")
                    }
                })
        }
    })
}))


//Add to favourite

let addToFavourite = document.querySelectorAll(".addtofavourite");
let clickCount = 1;
addToFavourite.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    url = btn.getAttribute("href")
    fetch(url).then(res => {
        if (res.status == 200) {
            Toastify({
                text: "Was executed",
                gravity: "bottom",
                duration: 3000,
                style: {
                    background: "grey"
                }
            }).showToast();
            
            
        }
        else alert("Unknown error")
    })
}))

//add to card item

let addToBasketBtnsCard = document.querySelectorAll(".addtobasketCard");

addToBasketBtnsCard.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    url = btn.getAttribute("href")
    fetch(url).then(res => {
        if (res.status == 200) {
            Toastify({
                text: "Added to shopping cart",
                gravity: "bottom",
                duration: 3000,
                style: {
                    background: "green"
                }
            }).showToast();
            var input = btn.parentNode.querySelector('.countSpanCard');
            var currentValue = parseInt(input.value);
            var newValue = currentValue + 1;
            input.value = newValue;

        }
        else alert("Unknown error")
    })
}))

//delete card item
document.querySelectorAll(".btn-deleteproductCard").forEach((btn, i) => btn.addEventListener("click", function (e) {
    e.preventDefault();
    fetch(btn.getAttribute("href")).then(response => response.json()).then(response => {
        Toastify({
            text: "Deleted from shopping cart",
            gravity: "bottom",
            duration: 3000,
            style: {
                background: "red"
            }
        }).showToast();
        console.log(response[i])
        if (response[i].count <= 0) btn.parentElement.parentElement.parentElement.remove()
        else {
            //btn.parentElement.querySelector("#basketItemCount").innerHTML = response[i].count
            btn.parentElement.querySelector(".countSpanCard").value = response[i].count
        }
        
        btn.parentElement.querySelector(".countSpanCard").value = response[i].count
        //spanPrice = document.querySelector(".totalPrice")
        //spanPrice2 = document.querySelector(".totalPrice2")
        //fetch("/home/getbasketcount")
        //    .then(response => response.json())
        //    .then(res => {
        //        console.log(res)
        //        span.innerHTML = res.count;
        //        spanPrice.innerHTML = res.price;
        //        spanPrice2.innerHTML = res.price
        //    })
    })
}))


const selectSort = document.querySelector('#sort');

selectSort.addEventListener('change', function (event) {
    event.preventDefault();
    //alert("success")
    const url = event.target.value;
    if (url !== '') {
        fetch(url).then(response => response.text())
            .then((html1) => {
                filterBlock = document.querySelector(".filterProductsBlock");
                
                filterBlock.innerHTML = html1;
                console.log(html1)
                console.log("success")
            })
            
    }
});