   M S B A M L   `   `   `  ����   PerfMon    PerfMon.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Ced4http://schemas.microsoft.com/expression/2010/drawing     -     �Name$  Window�� � Title$ PerfMon��$	��232��$	��258��$��
5, 5, 5, 0q�$��Center=�$��Center=� � Icon$- '/PerfMon;component/Images/Sign_Icon.ico�� � 
ResizeMode$ CanMinimize=� � WindowStartupLocation$ CenterScreen=�.��� -   $��grid1��$��5q� � RowDefinitions �� $
��Auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* � � ColumnDefinitions �� $
��Auto ��� $
��auto ��� $
��auto ��� $��* �.+��� $��설정��$
��Bold"�.���� $��0��$��2�� �� Orientation$ Vertical=�.+��� $��35��$��Center=�$ 
Horizontal=�.+��� -   $��SetTimeLabel��$��25��$��70��$��5q�$��Center=�$��Normal"�.��시간(분)�� -   $��SetTime��$��25��$��55��$��5,5,0,5q� �� TextAlignment$ Center=�$��Normal"�	 � ToolTip$	 1~999��$��30���� $ Vertical=�.+��� -   $��RepeatUpButton��$��12��$��15��$��0,5,5,0q�
 �� Delay$	
 200�� �� Interval$ 1��$��5(�$��0q�$	��Top=�$��Stretch=�$��0q� �� 	ClickMode$ Release=�$��Normal"�.��▲�� -   $��RepeatDownButton��$��12��$��15��$��0,0,5,5q�$	
 200��$ 1��$��5(�$��0q�$��Center=�$��0q�$ Release=�$��Normal"�.��▼�� $��35��$��Center=�$ 
Horizontal=�.+��� -   $��FolderLabel��$��25��$��70��$��5q�$��Center=�$��Normal"�.��저장 경로�� -   $��DirectoryButton��$��25��$��70��$��5q�$��Normal"�.��찾아보기�� $��Center=�$ 
Horizontal=�.+�	� -	   $��SA1RadioBurron�� 	� 	GroupName$ ProcessRadio��$��0, 0, 20, 0q� d� 	IsChecked$
 TrueY�.��SA1	� -
   $��SA2RadioBurron��$ ProcessRadio��.��SA2�� -   $��	MainImage��$.��(/PerfMon;component/Images/Sign_Image.jpg��$��0��$��2��$��60��$��60��$��5, 10, 5, 5q�$��Center=�$	��Top=��� -   $��Timer��$��2��$��25��$��60��$	��Top=�$��13(�$��4,67,5,0q�$��Center=�.��
00:00:00�� -   $��	Directory��$��2��$��4��$��25��$	��230��$��
5, 5, 5, 0q�$
��Left=�$
 Left=�.��D:\PerfMon\�� $��3��$��4��$��35��$��Center=�$ 
Horizontal=�.+��� -   $��StartButton��$��25��$��70��$��5,0,10,0q�.��시작�� -   $��	EndButton��$��25��$��70��$��10,0,5,0q�.��종료�� -   $��Director��$��4��$��4��$��25��$��Center=�$	��Top=�$��Center=�.��MoRoo� -   $��Progress��$��4��$��4��$��12��$	��230��$��0,8,0,0q� J� Opacity$ 0Q�$��Center=��� -   $��ProgressValue��$��4��$��4��$��25��$��Center=�$	��Top=�$��Center=�� -   $��OverNumberAllert����.  $
��Left=� � VerticalOffset$	 -40�� � HorizontalOffset$ -5�� � 	StaysOpen .  $
��Fade=� � AllowsTransparency .  � PlacementTarget ��  �� ElementName$ SetTime��.���� $	��180��$��50��.+�F� -    F�Name$ 
Container1�� �� Left$ 0��
 �� Top$ 0�����#FFFFFF���#A9A9A9O���� ��
 Zbр�!
 Zbр�!
����    ����        р�!
    р�!
���� ��
 Zb	�� $4��./PerfMon;component/Images/red_warning_sign.jpg��$ 8��$ 12���� $ 38��$ 16��$	��210��$
��Bold"�$!��최대치 초과! (1 ~ 999)��� -   $��NoNumberAllert����.  $
��Left=�$	 -40��$ -5�� .  $
��Fade=� .  �� $ SetTime��.���� $	��180��$��50��.+�F� -   $ 
Container2��$ 0��$ 0�����#FFFFFF���#A9A9A9O���� ��
 Zbр�!
 Zbр�!
����    ����        р�!
    р�!
���� ��
 Zb	�� $4��./PerfMon;component/Images/red_warning_sign.jpg��$ 8��$ 12���� $ 38��$ 16��$	��210��$
��Bold"�$��숫자만 입력 가능!��� -   $��FolderAllert����.  $
��Left=�$	 -40��$ -5�� .  $
��Fade=� .  �� $ 	Directory��.���� $	��180��$��50��.+�F� -   $ 
Container3��$ 0��$ 0�����#FFFFFF���#A9A9A9O���� ��
 Zbр�!
 Zbр�!
����    ����        р�!
    р�!
���� ��
 Zb	�� $4��./PerfMon;component/Images/red_warning_sign.jpg��$ 8��$ 12���� $ 38��$ 8��$	��210��$
��Bold"�$��입력하신 문자는���� $ 38��$ 23��$	��210��$
��Bold"�$!��사용할 수 없습니다.��