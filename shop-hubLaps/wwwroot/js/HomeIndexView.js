document.addEventListener("DOMContentLoaded", function () {
    const rangeMin = document.getElementById("range-min");
    const rangeMax = document.getElementById("range-max");
    const pMin = document.getElementById("pmin");
    const pMax = document.getElementById("pmax");

    // Hàm định dạng số với dấu phẩy
    function formatNumberWithCommas(num) {
        return num.toLocaleString('it-IT');
    }

    // Cập nhật giá trị khi người dùng chỉnh thanh trượt
    function updateRangeValues() {
        const minVal = parseInt(rangeMin.value);
        const maxVal = parseInt(rangeMax.value);

        // Cập nhật giá trị trong ô nhập với định dạng có dấu phẩy
        pMin.value = formatNumberWithCommas(minVal);
        pMax.value = formatNumberWithCommas(maxVal);
    }

    // Khi người dùng chỉnh sửa giá trị ô nhập, đồng bộ lại với thanh trượt
    function updateInputValues() {
        const minInput = parseInt(pMin.value.replace(/,/g, "")) || rangeMin.min;
        const maxInput = parseInt(pMax.value.replace(/,/g, "")) || rangeMax.max;

        // Cập nhật giá trị của thanh trượt
        rangeMin.value = minInput;
        rangeMax.value = maxInput;

        // Đảm bảo giá trị trong khoảng hợp lệ
        if (minInput >= rangeMin.min && minInput <= rangeMax.max) {
            pMin.value = formatNumberWithCommas(minInput);
        }
        if (maxInput >= rangeMin.min && maxInput <= rangeMax.max) {
            pMax.value = formatNumberWithCommas(maxInput);
        }
    }

    // Gán sự kiện cho thanh trượt và ô nhập
    rangeMin.addEventListener("input", updateRangeValues);
    rangeMax.addEventListener("input", updateRangeValues);
    pMin.addEventListener("input", updateInputValues);
    pMax.addEventListener("input", updateInputValues);

    // Khởi tạo giá trị hiển thị khi tải trang
    updateRangeValues();
});



document.addEventListener("DOMContentLoaded", function () {
    const filterLinks = document.querySelectorAll('.v5-hl-list-options a');
    const laptopContainer = document.getElementById('laptopContainer'); // Lấy container chứa danh sách laptop

    filterLinks.forEach(link => {
        link.addEventListener('click', function (event) {
            event.preventDefault(); // Ngừng hành động mặc định của liên kết (chuyển hướng)

            const nhuCauId = this.getAttribute('data-id');
            console.log('Nhu cầu ID đã chọn:', nhuCauId);

            // Hiển thị trạng thái đang tải (có thể thêm loader hoặc text "Đang tải...")
            laptopContainer.innerHTML = '<p>Đang tải...</p>';

            // Gửi yêu cầu AJAX để lấy laptop theo nhu cầu
            fetch(`/Home/GetLaptopsByNhuCau?nhuCauId=${nhuCauId}`)
                .then(response => response.json())
                .then(data => {
                    laptopContainer.innerHTML = ''; // Xóa các sản phẩm cũ

                    if (data && data.length > 0) {
                        data.forEach(laptop => {
                            // Format giá gốc (giá chưa có thuế) và giá thực tế (có thuế)
                            const originalPrice = laptop.giaban ? laptop.giaban.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' }) : 'Liên hệ';
                            const discountedPrice = laptop.giaban ? (laptop.giaban * 1.2).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' }) : null;

                            const laptopHTML = `

                                    <div class="col-12 col-sm-6 col-md-5 col-lg-4 mb-4 box-container">
                                        <div class="card h-100 box-card">
                                            <a class="card-img" href="/laptop/${laptop.tenlaptop}" title="${laptop.tenlaptop}">
                                                <img src="${laptop.hinh}" class="card-img-top" alt="${laptop.tenlaptop}">
                                            </a>
                                            <div class="card-body">
                                                <h5 class="card-title">
                                                    <a href="/laptop/${laptop.tenlaptop}" class="text-limit">${laptop.tenlaptop}</a>
                                                </h5>
                                               <div class="price">
                                                    <strong>${discountedPrice}</strong>
                                                    ${laptop.giaban ? `<strike>${originalPrice}</strike>` : ''}
                                                </div>
                                                <div class="specs">
                                                    <ul class="list-unstyled">
                                                        <li><i class="bi bi-cpu"></i> ${laptop.cpu}</li>
                                                        <li>
                                                            <svg fill="#000000" width="20px" height="20px" viewBox="0 0 1069 1069" style="fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;" version="1.1" xml:space="preserve" xmlns="http://www.w3.org/2000/svg" xmlns:serif="http://www.serif.com/" xmlns:xlink="http://www.w3.org/1999/xlink">
                                                                <rect height="1066.67" id="Vga-computer-alt2" style="fill:none;" width="1066.67" x="1.117" y="0.592"></rect><g>
                                                                <path d="M818.374,339.599c0.385,-0.014 0.774,-0.021 1.164,-0.021c-0,0 43.86,0 43.86,0c31.553,-0 61.813,12.534 84.124,34.846c22.311,22.311 34.846,52.571 34.845,84.124c0,80.892 0,200.34 0,281.231c0.001,31.553 -12.534,61.814 -34.845,84.125c-22.311,22.311 -52.572,34.846 -84.125,34.845l-366.5,0c-17.259,0 -31.25,-13.991 -31.25,-31.25c0,-17.258 13.992,-31.25 31.25,-31.25c17.073,0 33.229,-7.723 43.951,-21.013l52.597,-65.196c22.585,-27.994 56.623,-44.268 92.592,-44.268l45.782,-0c14.976,-0 29.339,-5.95 39.929,-16.54c10.591,-10.59 16.54,-24.953 16.54,-39.93l-0,-207.203l-69.709,0c-17.247,0 -31.25,-14.002 -31.25,-31.25c-0,-17.247 14.003,-31.25 31.25,-31.25l99.795,0Zm-214.175,456.65l37.888,-46.964c10.722,-13.289 26.877,-21.012 43.949,-21.013c0.003,-0 45.783,-0 45.783,-0c31.552,-0 61.813,-12.534 84.124,-34.846c22.311,-22.311 34.845,-52.571 34.845,-84.124l-0,-207.224l12.61,0c14.977,0 29.34,5.949 39.93,16.54c10.59,10.59 16.539,24.953 16.539,39.93l0,281.232c0,14.977 -5.949,29.34 -16.539,39.93c-10.59,10.59 -24.954,16.54 -39.93,16.539l-259.199,0Zm-194.748,-491.491c-126.481,0 -229.167,102.686 -229.167,229.167c-0,126.481 102.686,229.167 229.167,229.167c126.48,-0 229.166,-102.686 229.166,-229.167c0,-126.481 -102.686,-229.167 -229.166,-229.167Zm-0,62.5c91.985,0 166.666,74.681 166.666,166.667c0,91.986 -74.681,166.667 -166.666,166.667c-91.986,-0 -166.667,-74.681 -166.667,-166.667c-0,-91.986 74.681,-166.667 166.667,-166.667Z" style="fill-opacity:0.5;"></path>
                                                                <path d="M850.788,323.684c-0,-30.389 -12.072,-59.534 -33.561,-81.023c-21.489,-21.488 -50.633,-33.561 -81.023,-33.561c-135.02,0 -400.066,0 -535.087,0c-63.282,0 -114.583,51.301 -114.583,114.584c-0,111.691 -0,308.791 -0,420.482c0,63.283 51.3,114.583 114.583,114.583l297.878,0c34.644,0 67.427,-15.674 89.18,-42.637l55.23,-68.459c9.887,-12.257 24.789,-19.381 40.536,-19.381l52.264,-0c63.283,-0.001 114.583,-51.301 114.583,-114.584l-0,-290.004Zm-62.5,-0l-0,290.004c-0,28.765 -23.318,52.083 -52.082,52.084c-0.003,-0 -52.264,-0 -52.264,-0c-34.646,0 -67.429,15.675 -89.181,42.637l-55.23,68.46c-9.887,12.255 -24.788,19.38 -40.535,19.38c-0.003,0 -297.879,0 -297.879,0c-28.765,-0.001 -52.082,-23.318 -52.083,-52.081c-0,-0.004 -0,-420.484 -0,-420.484c0.001,-28.766 23.319,-52.084 52.083,-52.084c135.021,0 400.067,0 535.087,0c13.814,0 27.061,5.488 36.829,15.255c9.767,9.768 15.255,23.016 15.255,36.829Zm-397.652,241.521c0.789,2.955 1.199,6.025 1.199,9.138c-0,0.009 -0,0.018 -0,0.026c-0,9.403 -3.736,18.42 -10.384,25.069c-6.649,6.648 -15.666,10.383 -25.069,10.383l-0.026,0c-9.403,0 -18.42,-3.735 -25.069,-10.383c-12.195,-12.196 -31.998,-12.196 -44.194,-0c-12.196,12.196 -12.196,31.998 0,44.194c18.37,18.37 43.284,28.689 69.263,28.689l0.026,0c25.979,0 50.893,-10.319 69.263,-28.689c18.37,-18.37 28.69,-43.284 28.69,-69.263c-0,-0.008 -0,-0.017 -0,-0.026c-0,-13.724 -2.88,-27.15 -8.305,-39.47c2.123,-2.09 4.519,-3.921 7.143,-5.436c0.008,-0.005 0.016,-0.009 0.023,-0.014c8.143,-4.701 17.82,-5.975 26.902,-3.542c9.082,2.434 16.826,8.376 21.527,16.519c0.004,0.007 0.009,0.015 0.013,0.023c4.701,8.142 5.975,17.819 3.542,26.901c-4.464,16.66 5.437,33.81 22.097,38.274c16.66,4.464 33.809,-5.438 38.273,-22.097c6.724,-25.094 3.204,-51.83 -9.785,-74.328c-0.005,-0.008 -0.009,-0.016 -0.014,-0.023c-12.989,-22.499 -34.383,-38.915 -59.477,-45.639c-25.093,-6.724 -51.83,-3.204 -74.328,9.786c-0.007,0.004 -0.015,0.008 -0.023,0.013c-11.829,6.83 -21.977,15.983 -29.921,26.779c-3.017,-0.795 -5.943,-1.992 -8.694,-3.58c-0.007,-0.004 -0.015,-0.009 -0.023,-0.013c-8.143,-4.701 -14.084,-12.445 -16.518,-21.527c-2.433,-9.082 -1.159,-18.759 3.542,-26.902c0.004,-0.007 0.009,-0.015 0.013,-0.023c4.701,-8.143 12.445,-14.084 21.527,-16.518c16.66,-4.464 26.561,-21.613 22.097,-38.273c-4.464,-16.66 -21.614,-26.561 -38.273,-22.097c-25.094,6.724 -46.488,23.14 -59.477,45.638c-0.005,0.008 -0.009,0.016 -0.014,0.023c-12.989,22.498 -16.509,49.235 -9.785,74.328c6.724,25.094 23.14,46.488 45.638,59.477c0.008,0.005 0.016,0.009 0.023,0.014c11.957,6.903 25.111,11.132 38.578,12.569Z"></path>
                                                            </g></svg>                                                             
                                                            ${laptop.gpu}
                                                        </li>
                                                        <li><i class="bi bi-memory"></i>${laptop.ram}</li>
                                                        <li><i class="bi bi-hdd"></i>${laptop.hardware}</li>
                                                        <li><i class="bi bi-display"></i>${laptop.manhinh}</li>
                                                        <li><i class="fa-solid fa-battery-three-quarters"></i> ${laptop.pin}</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>`;
                            laptopContainer.insertAdjacentHTML('beforeend', laptopHTML);
                        });
                    } else {
                        laptopContainer.innerHTML = '<p>Không có laptop nào phù hợp với bộ lọc này.</p>';
                    }
                })
                .catch(error => {
                    console.error('Lỗi khi tải laptop:', error);
                    laptopContainer.innerHTML = '<p>Không thể tải laptop. Vui lòng thử lại sau.</p>';
                });
        });
    });
});
// Hàm để gọi API và tải danh sách laptop theo sortId
function sortLaptops(sortId) {
    const laptopContainer = document.getElementById('laptop-container');
    laptopContainer.innerHTML = 'Đang tải...';

    fetch(`/Home/GetLaptopsBySort?sortId=${sortId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Mã lỗi: ' + response.status);
            }
            return response.json();
        })
        .then(data => {
            laptopContainer.innerHTML = ''; // Xóa các sản phẩm cũ
            if (data && data.length > 0) {
                data.forEach(laptop => {
                    const laptopElement = document.createElement('div');
                    laptopElement.classList.add('laptop-item');
                    laptopElement.innerHTML = `
                            <h3>${laptop.name}</h3>
                            <p>Giá: ${laptop.price}</p>
                        `;
                    laptopContainer.appendChild(laptopElement);
                });
            } else {
                laptopContainer.innerHTML = '<p>Không có laptop nào phù hợp với bộ lọc này.</p>';
            }
        })
        .catch(error => {
            console.error('Lỗi khi tải laptop:', error);
            laptopContainer.innerHTML = `<p>Không thể tải laptop. Lỗi: ${error.message}</p>`;
        });
}


// Hàm gọi API và hiển thị laptop sau khi sắp xếp
// Hàm gọi API và hiển thị laptop sau khi sắp xếp
function sortLaptops(sortId) {
    // Gọi API để lấy danh sách laptop theo sortId
    fetch(`/Home/GetLaptopsBySort?sortId=${sortId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json(); // Chuyển đổi phản hồi thành JSON
        })
        .then(data => {
            console.log(data); // In dữ liệu nhận được từ API để kiểm tra

            // Tìm phần tử laptop-container và làm mới nội dung
            var laptopContainer = document.getElementById("laptop-container");
            laptopContainer.innerHTML = ""; // Xóa danh sách laptop hiện tại

            // Hiển thị danh sách laptop sau khi sắp xếp
            if (data.length > 0) {
                data.forEach(laptop => {
                    var laptopItem = document.createElement("div");
                    laptopItem.classList.add("laptop-item"); // Optional, thêm class để dễ dàng style
                    laptopItem.innerHTML = `
                            <h3>${laptop.tenlaptop}</h3>
                            <p>${laptop.mota}</p>
                            <p>Giá: ${laptop.giaban}</p>
                            <img src="${laptop.hinh}" alt="${laptop.tenlaptop}" width="100" />
                        `;
                    laptopContainer.appendChild(laptopItem);
                });
            } else {
                laptopContainer.innerHTML = "<p>Không có laptop nào phù hợp.</p>";
            }
        })
        .catch(error => {
            console.error('Error:', error); // Log lỗi cụ thể
            alert("Có lỗi khi tải danh sách laptop. Vui lòng thử lại.");
        });
}



