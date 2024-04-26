import telebot;
from telebot import types;

class Record : 
    def __init__(self) : 
        self.dynamicIndex = 0;
        self.indices = [0];
        self.username = "";
        self.password = "";
        self.productName = "";
        self.productDescription = "";
        self.productImageIDs = [];
        self.authorized = False;

TOKEN = "TOKEN";
tBot = telebot.TeleBot(TOKEN);
indices = {};

def AddProduct(callback : types.CallbackQuery) : 
    callback.message.from_user.username = callback.from_user.username;
    username = callback.message.from_user.username;
    indices[username].indices.append(5);
    CallActions(callback.message, False);
    return;

def ListProduct(callback : types.CallbackQuery) : 
    callback.message.from_user.username = callback.from_user.username;
    username = callback.message.from_user.username;
    indices[username].indices.append(7);
    productButtons = [];
    buttonIDs = [0, 1, 2, 3];
    productNames = ["Combiner", "Gloves", "Shovel", "Boots"];
    for productIndex in range(len(productNames)) : 
        currentProduct = types.InlineKeyboardButton(productNames[productIndex], callback_data=f"product{buttonIDs[productIndex]}");
        productButtons.append(currentProduct);
    productButtons.append(types.InlineKeyboardButton("Back", callback_data="back"));
    actionClasses[7].buttons = productButtons;
    CallActions(message=callback.message, document=False);
    return;

def UpdateProduct(callback : types.CallbackQuery) : 
    return;
def RemoveProduct(callback : types.CallbackQuery) : 
    return;

def Back(callback : types.CallbackQuery) : 
    callback.message.from_user.username = callback.from_user.username;
    username = callback.message.from_user.username;
    #indices[username] = max(0, indices[username] - 2);
    breakpoint();
    indices[username].indices.pop(-1);
    indices[username].indices.pop(-1);
    CallActions(callback.message, False);
    return;

actionHandlers = {
    "add" : AddProduct, 
    "list" : ListProduct, 
    "update" : UpdateProduct, 
    "remove" : RemoveProduct, 
    "back" : Back
};

class Action : 
    def __init__(self, nextIndex : int, callback=None) : 
        self.next = nextIndex;
        self.callback = callback;
    def Answer(self, message : telebot.types.Message) : 
        tBot.send_message(message.chat.id, "BOOTS");
        if(self.callback) : 
            self.callback(message);
class TextAction(Action) : 
    def __init__(self, nextIndex : int, text : str, callback=None) : 
        self.next = nextIndex;
        self.text = text;
        self.callback = callback;
    def Answer(self, message : telebot.types.Message) : 
        tBot.send_message(message.chat.id, text=self.text);
        if(self.callback) : 
            self.callback(message);
class ButtonAction(Action) : 
    def __init__(self, nextIndex : int, title : str, buttons : list[types.InlineKeyboardButton], callback=None) : 
        self.next = nextIndex;
        self.title = title;
        self.buttons = buttons;
        self.callback = callback;
    def Answer(self, message : telebot.types.Message) : 
        container = types.InlineKeyboardMarkup(row_width=1);
        for button in self.buttons : 
            container.add(button);
        if(self.callback) : 
            if(not self.callback(message)) : 
                tBot.send_message(message.chat.id, text=self.title, reply_markup=container);
        else : 
            tBot.send_message(message.chat.id, text=self.title, reply_markup=container);
class PhotoAction(Action) : 
    def __init__(self, nextIndex: int, callback=None):
        self.next = nextIndex;
        self.callback = callback;
    def Answer(self, message : types.Message) : 
        if(message.content_type == "photo") : 
            tBot.send_message(message.chat.id, "Received The Image");
            if(self.callback) : 
                self.callback(message);
            return;
        #indices[message.from_user.username].indices.pop(-1); #= max(0, indices[message.from_user.username] - 1);
        tBot.send_message(message.chat.id, "Only Compressed Images Are Accepted !");

def ProductTitle(message : types.Message) : 
    username = message.from_user.username;
    title = message.text;
    indices[username].productTitle = title;
    print(f"Title : {title};");
    return False;

def ProductDescription(message : types.Message) : 
    username = message.from_user.username;
    description = message.text;
    indices[username].productDescription = description;
    print(f"Description : {description};");
    return False;

def ProductUsername(message : types.Message) : 
    username = message.from_user.username;
    userName = message.text;
    indices[username].username = userName;
    indices[username].authorized = False;
    print(f"Username : {userName};");
    return False;

def ProductPassword(message : types.Message) : 
    username = message.from_user.username;
    password = message.text;
    #indices[username].password = password;
    record = indices[username];
    record.password = password;
    if((record.username != "Boots" or record.password != "12345") and not record.authorized) : 
        tBot.send_message(message.chat.id, "Invalid Credentials !");
        #indices[username].indices = [];
        record.indices = [];
        #indices[username].indices.append(0);
        record.indices.append(0);
        CallActions(message, False, append=False);
        print(f"Password : {password}, Username : {record.username};");
        return True;
    record.authorized = True;
    return False;

def ProductImage(message : types.Message) : 
    username = message.from_user.username;
    imageID = message.photo[-1].file_id;
    indices[username].productImageIDs.append(imageID);
    print(f"ImageID : {imageID};");
    return False;

def ProductPhotoHandler(message : types.Message)  :
    if(message.content_type == "photo") : 
        tBot.send_message(message.chat.id, "Received The Image");
        return;
    tBot.send_message(message.chat.id, "Only Compressed Images Are Accepted !");

def InitializeActions() : 
    firstAction = ButtonAction(1, "Enter the Username : ", buttons=[types.InlineKeyboardButton("Exit", callback_data="exit")]);
    secondAction = ButtonAction(2, "Enter the Password : ", buttons=[types.InlineKeyboardButton("Back", callback_data="back")], callback=ProductUsername);

    thirdButtons = [];
    thirdButtons.append(types.InlineKeyboardButton(text="Add", callback_data="add"));
    thirdButtons.append(types.InlineKeyboardButton(text="List", callback_data="list"));
    #thirdButtons.append(types.InlineKeyboardButton(text="Update", callback_data="update"));
    #thirdButtons.append(types.InlineKeyboardButton(text="Remove", callback_data="remove"));
    thirdButtons.append(types.InlineKeyboardButton(text="Back", callback_data="back"));
    thirdAction = ButtonAction(2, title="List of Items : ", buttons=thirdButtons, callback=ProductPassword);
    
    fourthAction = ButtonAction(4, title="Upload the Image(s) for the Product", buttons=[types.InlineKeyboardButton("Back", callback_data="back")], callback=ProductDescription);
    fifthAction = PhotoAction(5, callback=ProductImage);

    # sixthButtons = [];
    # sixthButtons.append(types.InlineKeyboardButton(text="Back", callback_data="back"));
    sixthAction = ButtonAction(6, title="Insert the title for the product : ", buttons=[types.InlineKeyboardButton(text="Back", callback_data="back")]);
    seventhAction = ButtonAction(3, title="Insert the description for the product : ", buttons=[types.InlineKeyboardButton("Back", callback_data="back")], callback=ProductTitle);
    eighthAction = ButtonAction(7, "Product List", buttons=[]);
    #actions = [firstAction, secondAction, thirdAction];
    return ([firstAction.Answer, 
             secondAction.Answer, 
             thirdAction.Answer, 
             fourthAction.Answer, 
             fifthAction.Answer, 
             sixthAction.Answer, 
             seventhAction.Answer, 
             eighthAction.Answer], 
    [firstAction, 
     secondAction, 
     thirdAction, 
     fourthAction, 
     fifthAction, 
     sixthAction, 
     seventhAction, 
     eighthAction]);

actions, actionClasses = InitializeActions();

def PhotoHandler(message) : 
    fileID = message.photo[-1].file_id;
    FILE = tBot.get_file(fileID);
    fileBytes = tBot.download_file(FILE.file_path);
    content = FILE.file_path.split(".");
    extension = content[len(content) - 1];
    with open(f"Photos/{FILE.file_id}.{extension}", "wb") as f : 
        f.write(fileBytes);
    tBot.send_photo(message.chat.id, fileBytes);

callbacks = {"photo" : PhotoHandler};

def CallActions(message : types.Message, document : bool, append = True) : 
    if(message.text == "/start" or (not message.from_user.username in indices)) : 
        #indices[message.from_user.username] = 0;
        indices[message.from_user.username] = Record();
    #dynamicIndex = indices[message.from_user.username];
    dynamicIndex = indices[message.from_user.username].indices[-1];
    if(document) : 
        if(actionClasses[dynamicIndex].__class__.__name__ == PhotoAction.__name__) : 
            if(indices[message.from_user.username].indices[-1] != actionClasses[dynamicIndex].next) : 
                indices[message.from_user.username].indices.append(actionClasses[dynamicIndex].next);
            actions[dynamicIndex](message);
            #indices[message.from_user.username] += 1;
            return;
    else : 
        #indices[message.from_user.username] += 1;
        try : 
            occurrence = 0;
            if(len(indices[message.from_user.username].indices) >= 2) : 
                occurrence += 1 - min(abs(indices[message.from_user.username].indices[-1] - actionClasses[dynamicIndex].next), 1);
                occurrence += 1 - min(abs(indices[message.from_user.username].indices[-2] - actionClasses[dynamicIndex].next), 1);
            if(len(indices[message.from_user.username].indices) < 2 or occurrence < 2) :
                indices[message.from_user.username].indices.append(actionClasses[dynamicIndex].next);
        except : 
            print("[!] Error;");
        # if(indices[message.from_user.username].indices[-1] != actionClasses[dynamicIndex].next) :
        #     indices[message.from_user.username].indices.append(actionClasses[dynamicIndex].next);
        actions[dynamicIndex](message);
        #print(indices[message.from_user.username].indices);

@tBot.message_handler()
def ShowBobs(message : types.Message) : 
    CallActions(message, False);
    '''dynamicIndex = indices[message.from_user.username];
    actions[dynamicIndex].Answer(message);
    indices[message.from_user.username] += 1;'''

@tBot.message_handler(content_types=["document", "photo", "video", "voice", "audio"])
def HandleFiles(message : types.Message) : 
    CallActions(message, True);
    #callbacks[message.content_type](message);

@tBot.callback_query_handler(func=lambda call:True)
def CallbackHandler(callback : types.CallbackQuery) : 
    if(callback.message) : 
        if(callback.data in actionHandlers) : 
            actionHandlers[callback.data](callback);

tBot.polling();