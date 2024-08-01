mp.events.add('render', () => {

    const playerPos = mp.players.local.position;
    const rentalLocation = new mp.Vector3(200.0, 300.0, 20.0);

    if (playerPos.distanceTo(rentalLocation) < 5.0) {
        mp.game.graphics.drawText("Нажмите ~g~E~w~, чтобы арендовать автомобиль", [0.5, 0.8], {
            font: 4,
            color: [255, 255, 255, 185],
            scale: [0.7, 0.7],
            outline: true
        });

        if (mp.keys.isDown(0x45)) { 
            mp.events.callRemote('showRentalMenu');
        }
    }
});

mp.events.add('showRentalMenu', () => {

    mp.gui.chat.push("~g~Показать меню аренды (пример, требуется реализация UI).");
});

mp.events.add('notification', (message) => {
    mp.gui.chat.push(message);
});
