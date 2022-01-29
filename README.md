<p style="text-align: center;">
  
  # Metaseed-unity-toolkit
  
  Helps develop games on Unity3d &amp; NEAR blockchain in two clicks

  The plugin is zipped in a “.unitypackage” file in folder “releases/”, and if you need the sources of the package file, they can be found in “source/”.
  There are 2 demo scenes - one with code, and the second one with components.
  
  **Change your project's .NET version to 4.x if you have any errors after importing the plugin**
  (Edit -> Project Settings -> Player -> Api Compatibility Level)
  
  ### If you find any bugs or need some help feel free to contact us
  [<img src="https://img.shields.io/badge/Telegram-2CA5E0?style=for-the-badge&logo=telegram&logoColor=white" />](https://t.me/metaseed_near)
  
  ### Also, subscribe to receive our newest announcements and updates
  [<img src="https://img.shields.io/badge/Twitter-1DA1F2?style=for-the-badge&logo=twitter&logoColor=white" />](https://twitter.com/metaseed_near)
  
  # Tutorials
  
  [Tutorial Part 1 (Demo scenes overview) [RU]](https://www.youtube.com/watch?v=36YzP561ST8)
  
  [Tutorial Part 2 Deploying NFT contract [RU]](https://www.youtube.com/watch?v=Yo0zT2I28Sg)


  ## Example

  We have two types of connections or actors in ower Toolkit.
  They are “Player” and “Developer”.

  As you know, a standard NEAR NFT specification requires initializing the contract by the owner, who will have a monopolistic ability to mint tokens. The cost of     every token mintage is around 0.1 NEAR due to the storage [cost](https://docs.near.org/docs/tutorials/contracts/nfts/minting-nfts#).

  In our Toolkit, you can set up the process in the following way.

  For example, you want to give your player an NFT after successfully ending each level. To make it possible, you need to have an NFT contract with your Developer     account being an owner of this contract so that an unlimited amount of NFTs could be mint.

  After that, a player has to log in to his account on NEAR, the account where NFTs will be transferred.

  Then at the end of each level, you could transfer 0.1 Near (the cost of creating your NFT) from the player's account to the Developer account. And mint an NFT       with transferring it to the player using your Developer account.

  If you want to bear the cost of NFT creation you could just skip the steps and even don’t force your user to log in.

  The developer account is connected in the setting and used throughout the entire game. The credentials are saved safely inside the build.
   
  ## Components overview
  
  
  
  > Note that all toolkits components can be called via Unity scripting like regular components.
  
  
  
  Firstly you need to connect your NEAR wallet
  When the .unitypackage file is installed in your project, “Near” tab will appear at the top menu.
  Click on “Developer Account” and then on “Connect Wallet” 

  ![Alt text](/screenshots/7.jpg)

  ![Alt text](/screenshots/1.jpg)

  After login, you will receive a message - “Successfully, now please return to the editor you are connected!”

  Open Unity to see that your account is connected.

  Now, after connecting, you can create your first NFT via a Simple NFT publisher. Fill in the fields: Title, Description, TokenId, Link to the media. The receiver   id is a NEAR   wallet address.

  ![Alt text](/screenshots/2.jpeg)

  Then, in Contract Caller, you can do any logic for interaction between the game and NFT contract so that your game rewards players with NFT’s for certain in-game   actions, for example killing the boss.

  ![Alt text](/screenshots/3.jpeg)

  It could be both minting a new NFT or sending a pre-minted NFT to a player. 
  You also could send NEAR from the player account.
  In order to send the NEAR you just need to specify the receiver’s address and an amount to send.

  ![Alt text](/screenshots/5.jpeg)
</p>
