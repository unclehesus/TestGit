const rentalVehicles = []; 


const availableVehicles = [
    { model: 'Blista', price: 100 }, 
    { model: 'Faggio', price: 50 },
    { model: 'Sultan', price: 200 }
];


mp.events.addCommand('rent', (player, fullText, vehicleName) => {
    if (!vehicleName) {
        player.outputChatBox("Использование: /rent [model]");
        return;
    }

    const vehicleData = availableVehicles.find(v => v.model.toLowerCase() === vehicleName.toLowerCase());
    if (!vehicleData) {
        player.outputChatBox("Такой модели нет в аренде.");
        return;
    }

    if (player.money < vehicleData.price) { 
        player.outputChatBox("У вас недостаточно средств для аренды этой машины.");
        return;
    }

    player.money -= vehicleData.price;
    const vehicle = mp.vehicles.new(mp.joaat(vehicleData.model), player.position, {
        heading: player.heading,
        numberPlate: 'RENTAL',
        color: [[255, 255, 255], [255, 255, 255]]
    });

    vehicle.setVariable('owner', player); 
    rentalVehicles.push(vehicle);
    player.putIntoVehicle(vehicle, -1); 

    player.outputChatBox(`Вы арендовали ${vehicleData.model} за ${vehicleData.price}$.`);
});


mp.events.addCommand('return', (player) => {
    const vehicle = player.vehicle;
    if (vehicle && vehicle.getVariable('owner') === player) {
        vehicle.destroy();
        rentalVehicles.splice(rentalVehicles.indexOf(vehicle), 1);
        player.outputChatBox("Вы вернули арендованный автомобиль.");
    } else {
        player.outputChatBox("Вы не в арендованной машине.");
    }
});


setInterval(() => {
    rentalVehicles.forEach((vehicle, index) => {
        if (!vehicle || !vehicle.getOccupants().length) { 
            vehicle.destroy(); 
            rentalVehicles.splice(index, 1);
        }
    });
}, 60000); 


mp.events.add('playerJoin', (player) => {
    player.money = 1000; 
});

mp.events.add('playerQuit', (player) => {
    rentalVehicles.forEach((vehicle, index) => {
        if (vehicle.getVariable('owner') === player) {
            vehicle.destroy();
            rentalVehicles.splice(index, 1);
        }
    });
});



mp.events.add('showRentalMenu', (player) => {
    player.call('notification', ['Меню аренды: /rent [model]']);
});
