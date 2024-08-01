const fireSpots = [
    { x: 200.0, y: 300.0, z: 20.0 }, // координаты точек пожаров
    { x: 250.0, y: 350.0, z: 20.0 },
    // Добавьте больше точек по мере необходимости
];

const activeFires = [];

function createFire(position) {
    const fire = mp.colshapes.newCircle(position.x, position.y, 10.0); // Создаем зону пожара
    activeFires.push({ position, colshape: fire });
    mp.players.call('fireCreated', [position.x, position.y, position.z]); // Сообщаем всем игрокам о пожаре
}

function removeFire(fire) {
    fire.colshape.destroy(); // Удаляем зону пожара
    activeFires.splice(activeFires.indexOf(fire), 1);
    mp.players.call('fireExtinguished', [fire.position.x, fire.position.y, fire.position.z]); // Сообщаем игрокам об устранении пожара
}

setInterval(() => {
    if (activeFires.length < 3) { // Ограничиваем количество активных пожаров
        const spot = fireSpots[Math.floor(Math.random() * fireSpots.length)];
        createFire(spot);
    }
}, 60000); // Каждую минуту пытаемся создать новый пожар
mp.events.addCommand('extinguish', (player) => {
    if (!player.hasData('firefighter')) {
        return player.outputChatBox('Вы не пожарный!');
    }

    const nearbyFire = activeFires.find(fire => fire.colshape.isPointWithin(player.position));
    if (nearbyFire) {
        removeFire(nearbyFire);
        player.outputChatBox('Пожар потушен!');
    } else {
        player.outputChatBox('Рядом нет пожаров.');
    }
});
mp.events.addCommand('firefighter', (player) => {
    player.setData('firefighter', true);
    player.outputChatBox('Вы начали работу пожарного.');
});

mp.events.addCommand('endfirefighter', (player) => {
    player.setData('firefighter', false);
    player.outputChatBox('Вы закончили работу пожарного.');
});
